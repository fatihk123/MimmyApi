param(
    [string]$ApiUrl = 'http://localhost:5245',
    [string]$WebUrl = 'http://localhost:5148',
    [switch]$AllowWrites
)
$ErrorActionPreference = 'Stop'
$script:checks = 0
function Check([bool]$Condition, [string]$Description) {
    if (-not $Condition) { throw "FAILED: $Description" }
    $script:checks++
    Write-Output "PASS: $Description"
}
function Request([string]$Method, [string]$Path, $Payload) {
    $options = @{ Uri = "$ApiUrl/$Path"; Method = $Method; SkipHttpErrorCheck = $true }
    if ($null -ne $Payload) { $options.Body = ConvertTo-Json -InputObject $Payload -Depth 5; $options.ContentType = 'application/json' }
    Invoke-WebRequest @options
}
$routes = @('/', '/Dashboard/Index', '/Category/CategoryList', '/Product/ProductList', '/Feature/FeatureList', '/About/AboutList', '/WhyChooseMimmy/WhyChooseMimmyList', '/Testimonial/TestimonialList', '/MimmyEvent/MimmyEventList', '/Chef/ChefList', '/Reservation/ReservationList', '/Reservation/CreateReservation', '/Gallery/ImageList', '/Gallery/CreateImage', '/Contact/ContactList', '/Message/MessageList', '/AI/CreateRecipeWithOpenAI', '/AI/DailyMenu', '/Chat/SendChatWithAI')
foreach ($route in $routes) {
    $response = Invoke-WebRequest -Uri "$WebUrl$route" -SkipHttpErrorCheck
    Check ($response.StatusCode -eq 200) "GET $route"
}
foreach ($route in @('/Feature/UpdateFeature/1', '/About/UpdateAbout/1', '/Chef/UpdateChef/1', '/Testimonial/UpdateTestimonial/1', '/MimmyEvent/UpdateMimmyEvent/1', '/WhyChooseMimmy/UpdateWhyChooseMimmy/1', '/Contact/UpdateContact/1')) {
    $response = Invoke-WebRequest -Uri "$WebUrl$route" -SkipHttpErrorCheck
    if ($response.StatusCode -ne 404) { Check ($response.StatusCode -eq 200) "GET $route" }
}
Check ((Request GET 'api/Products/GetById?id=2147483647').StatusCode -eq 404) 'Missing product returns 404'
Check ((Request DELETE 'api/Products?id=2147483647').StatusCode -eq 404) 'Missing product deletion returns 404'
Check ((Invoke-WebRequest -Uri "$WebUrl/Category/DeleteCategory/2147483647" -SkipHttpErrorCheck).StatusCode -eq 405) 'Deletion through GET is rejected'
Check ((Invoke-WebRequest -Uri "$WebUrl/Category/DeleteCategory/2147483647" -Method POST -SkipHttpErrorCheck).StatusCode -eq 400) 'Missing antiforgery token is rejected'
if ($AllowWrites) {
    if ([uri]$ApiUrl -and ([uri]$ApiUrl).Host -notin @('localhost', '127.0.0.1')) { throw 'Write tests are restricted to localhost.' }
    $prefix = 'MimmySmoke-' + [guid]::NewGuid().ToString('N').Substring(0, 10)
    $categoryId = $null; $productId = $null; $reservationId = $null; $messageId = $null; $imageId = $null; $webReservationId = $null; $webMessageId = $null
    try {
        $category = @{ CategoryName = $prefix }
        Check ((Request POST 'api/Categories' $category).StatusCode -eq 200) 'Create category'
        $categoryId = @((Request GET 'api/Categories').Content | ConvertFrom-Json | Where-Object CategoryName -eq $prefix)[0].CategoryId
        $product = @{ ProductName = $prefix; ProductDescription = 'Smoke test product'; Price = 250; ImageUrl = '/yummy-red-1.0.0/assets/img/menu/menu-item-1.png'; CategoryId = $categoryId }
        Check ((Request POST 'api/Products' $product).StatusCode -eq 200) 'Create product'
        $productId = @((Request GET 'api/Products').Content | ConvertFrom-Json | Where-Object ProductName -eq $prefix)[0].ProductId
        $product.ProductId = $productId; $product.Price = 300
        Check ((Request PUT 'api/Products' $product).StatusCode -eq 200) 'Update product'
        Check (((Request GET "api/Products/GetById?id=$productId").Content | ConvertFrom-Json).Price -eq 300) 'Updated product persists'
        Check ((Request DELETE "api/Categories?id=$categoryId").StatusCode -eq 409) 'Category with products cannot be deleted'
        $invalidProduct = $product.Clone(); $invalidProduct.Price = -1
        Check ((Request POST 'api/Products' $invalidProduct).StatusCode -eq 400) 'Negative product price is rejected'
        $reservation = @{ NameSurname = $prefix; Email = 'smoke@example.com'; PhoneNumber = '0000000000'; ReservationDate = (Get-Date).AddDays(2).ToString('yyyy-MM-dd'); ReservationTime = '19:00'; CountofPeople = 2; Message = 'Smoke test reservation'; ReservationStatus = 'Beklemede' }
        $result = Request POST 'api/Reservations' $reservation
        Check ($result.StatusCode -eq 200) 'Create reservation'
        $reservationId = ($result.Content | ConvertFrom-Json).ReservationID
        $invalidReservation = $reservation.Clone(); $invalidReservation.CountofPeople = 0
        Check ((Request POST 'api/Reservations' $invalidReservation).StatusCode -eq 400) 'Zero guests rejected'
        $invalidReservation.CountofPeople = 2; $invalidReservation.ReservationDate = '2020-01-01'
        Check ((Request POST 'api/Reservations' $invalidReservation).StatusCode -eq 400) 'Past reservation rejected'
        $message = @{ NameSurname = $prefix; Email = 'smoke@example.com'; Subject = $prefix; MessageDetails = 'Smoke test message' }
        Check ((Request POST 'api/Messages' $message).StatusCode -eq 200) 'Create message'
        $messageId = @((Request GET 'api/Messages').Content | ConvertFrom-Json | Where-Object Subject -eq $prefix)[0].MessageId
        $image = @{ Title = $prefix; ImageUrl = '/yummy-red-1.0.0/assets/img/gallery/gallery-1.jpg' }
        Check ((Request POST 'api/Images' $image).StatusCode -eq 200) 'Create gallery image'
        $imageId = @((Request GET 'api/Images').Content | ConvertFrom-Json | Where-Object Title -eq $prefix)[0].ImageId
        foreach ($route in @("/Category/UpdateCategory/$categoryId", "/Product/UpdateProduct/$productId", "/Gallery/UpdateImage/$imageId", "/Reservation/UpdateReservation/$reservationId", "/Message/OpenMessage/$messageId")) {
            Check ((Invoke-WebRequest -Uri "$WebUrl$route" -SkipHttpErrorCheck).StatusCode -eq 200) "GET $route"
        }
        $session = [Microsoft.PowerShell.Commands.WebRequestSession]::new()
        $form = Invoke-WebRequest -Uri "$WebUrl/Product/CreateProduct" -WebSession $session
        $token = [regex]::Match($form.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
        Check (-not [string]::IsNullOrWhiteSpace($token)) 'Product form has antiforgery token'
        $invalidForm = @{ __RequestVerificationToken = $token; ProductName = $prefix; ProductDescription = 'Invalid form smoke test'; Price = '-1'; ImageUrl = '/test.png'; CategoryId = "$categoryId" }
        $formResponse = Invoke-WebRequest -Uri "$WebUrl/Product/CreateProduct" -Method POST -Body $invalidForm -WebSession $session -SkipHttpErrorCheck
        Check ($formResponse.StatusCode -eq 200 -and $formResponse.Content.Contains(('value="' + $categoryId + '"')) -and $formResponse.Content.Contains('validation-summary-errors')) 'Invalid product preserves form input and category choices'
        $homePageResponse = Invoke-WebRequest -Uri "$WebUrl/" -WebSession $session
        $homeToken = [regex]::Match($homePageResponse.Content, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"').Groups[1].Value
        $webReservation = $reservation.Clone(); $webReservation.NameSurname = $prefix + '-web'; $webReservation.Message = ''; $webReservation.__RequestVerificationToken = $homeToken
        $webResponse = Invoke-WebRequest -Uri "$WebUrl/Default/BookTable" -Method POST -Body $webReservation -WebSession $session -SkipHttpErrorCheck
        $webReservationId = @((Request GET 'api/Reservations').Content | ConvertFrom-Json | Where-Object NameSurname -eq ($prefix + '-web'))[0].ReservationID
        Check ($webResponse.StatusCode -eq 200 -and $webResponse.Content.Trim() -eq 'OK' -and $webReservationId -gt 0) 'Public reservation form saves booking without optional notes'
        $webMessage = $message.Clone(); $webMessage.Subject = $prefix + '-web'; $webMessage.__RequestVerificationToken = $homeToken
        $webResponse = Invoke-WebRequest -Uri "$WebUrl/Default/SendMessage" -Method POST -Body $webMessage -WebSession $session -SkipHttpErrorCheck
        $webMessageId = @((Request GET 'api/Messages').Content | ConvertFrom-Json | Where-Object Subject -eq ($prefix + '-web'))[0].MessageId
        Check ($webResponse.StatusCode -eq 200 -and $webResponse.Content.Trim() -eq 'OK' -and $webMessageId -gt 0) 'Public contact form saves message'
    } finally {
        foreach ($entry in @(@('Reservations',$webReservationId), @('Messages',$webMessageId), @('Images',$imageId), @('Messages',$messageId), @('Reservations',$reservationId), @('Products',$productId), @('Categories',$categoryId))) {
            if ($entry[1]) { Check ((Request DELETE "api/$($entry[0])?id=$($entry[1])").StatusCode -eq 200) "Clean up test $($entry[0])" }
        }
    }
}
Write-Output "$script:checks smoke checks passed."
