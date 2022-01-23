# Service Description  
## Auth  
> - ``POST /api/play-together/v1/auth/login ``  
>   ***Description***: Login all User Account
>
> - ``GET /api/play-together/v1/auth/check-exist-email ``  
>   ***Description***: Check Exist Email
>
> - ``POST /api/play-together/v1/auth/login-google-player ``  
>   ***Description***: Sign in with Google for PDlayer
>
> - ``POST /api/play-together/v1/auth/login-google-hirer ``  
>   ***Description***: Sign in with Google for Hirer
>
> - ``POST /api/play-together/v1/auth/register-admin ``  
>   ***Description***: Register Admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-charity ``  
>   ***Description***: Register Charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-player ``  
>   ***Description***: Register a Normal Player Account  
>
> - ``POST /api/play-together/v1/auth/register-hirer ``  
>   ***Description***: Register a Normal Hirer Account  
>
>

## Admin   
> - ``GET /api/play-together/v1/admins ``  
>   ***Description***: Get all Admins   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name  
>

## Charity  
> - ``GET /api/play-together/v1/charities ``  
>   ***Description***: Get all Charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, Search by Name  
>

## Player  
> - ``GET /api/play-together/v1/players ``  
>   ***Description***: Get all Players   
>   ***Role Access***: Admin, Hirer  
>   ***Extension***: Paging, Filter by Gender, Search by Name 
>

## Hirer 
> - ``GET /api/play-together/v1/hirers ``  
>   ***Description***: Get all Hirers   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name 
>
> - ``GET /api/play-together/v1/hirers/profile ``  
>   ***Description***: Get Hirer Profile  
>   ***Role Access***: Hirer  
> 
> - ``GET /api/play-together/v1/hirers/{id} ``  
>   ***Description***: Get a Hirer by Id   
>   ***Role Access***: Admin  
> 
>
