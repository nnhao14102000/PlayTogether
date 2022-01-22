# Service Description  

## Table Of Contents  

- [Go to the API](#api-below)
- [Service Description](#service-description)
    * [Table Of Contents](#table-of-contents)
    * [Authentication services](#auth)
    * [Admin services](#admin)
    * [Charity services](#charity)
    * [Player services](#player)
    * [Hirer services](#hirer)

    

> ## Auth  
> [🔙](#table-of-contents)   [⬇️](#api-below)
> - ``POST /api/play-together/v1/auth/login ``  
>   ***Description***: Login all user account
>
> - ``GET /api/play-together/v1/auth/check-exist-email ``  
>   ***Description***: Check exist email
>
> - ``POST /api/play-together/v1/auth/login-google-player ``  
>   ***Description***: Sign in with Google for player
>
> - ``POST /api/play-together/v1/auth/login-google-hirer ``  
>   ***Description***: Sign in with Google for hirer
>
> - ``POST /api/play-together/v1/auth/register-admin ``  
>   ***Description***: Register admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-charity ``  
>   ***Description***: Register charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/auth/register-player ``  
>   ***Description***: Register a normal player account  
>
> - ``POST /api/play-together/v1/auth/register-hirer ``  
>   ***Description***: Register a normal hirer account  
>
>

> ## Admin   
> [🔙](#table-of-contents)   [⬇️](#api-below)
> - ``GET /api/play-together/v1/admins ``  
>   ***Description***: Get all admins   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, search by name  
>

> ## Charity  
> [🔙](#table-of-contents)   [⬇️](#api-below)
> - ``GET /api/play-together/v1/charities ``  
>   ***Description***: Get all charities   
>   ***Role Access***: Admin, Player  
>   ***Extension***: Paging, search by name  
>

> ## Player  
> [🔙](#table-of-contents)   [⬇️](#api-below)
> - ``GET /api/play-together/v1/players ``  
>   ***Description***: Get all players   
>   ***Role Access***: Admin, Hirer  
>   ***Extension***: Paging, filter by gender, search by name 
>

> ## Hirer 
> [🔙](#table-of-contents)   [⬇️](#api-below)
> - ``GET /api/play-together/v1/hirers ``  
>   ***Description***: Get all hirers   
>   ***Role Access***: Admin  
>   ***Extension***: Paging, search by name 
>

> ## Api below 
> [🔙](#table-of-contents)