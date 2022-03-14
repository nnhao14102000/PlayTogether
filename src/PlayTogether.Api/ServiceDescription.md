<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
1. <a href="#services-description" target="_self">Title</a> <br>
2. <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
3. <a href="#account" target="_self">Account</a> <br>



<h2 id="account">Account <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/play-together/v1/accounts/login-user ``  
>   ***Description***: Login User Account  
>   ***Use for***: Login User and update status of User to Online  
>
> - ``POST /api/play-together/v1/accounts/login-admin ``  
>   ***Description***: Login Admin Account   
>
> - ``POST /api/play-together/v1/accounts/login-charity ``  
>   ***Description***: Login Charity Account  
>
> - ``POST /api/play-together/v1/accounts/login-google ``  
>   ***Description***: Sign in with Google
>
> - ``PUT /api/play-together/v1/accounts/logout ``  
>   ***Description***: Logout Hirer or Player  
>   ***Role Access***: User    
>   ***Use for***: Update status of User to Offline
>
> - ``POST /api/play-together/v1/accounts/register-admin ``  
>   ***Description***: Register Admin  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-charity ``  
>   ***Description***: Register Charity  
>   ***Role Access***: Admin  
>
> - ``POST /api/play-together/v1/accounts/register-user ``  
>   ***Description***: Register a user Account  
>
> - ``PUT /api/play-together/v1/accounts/change-password ``  
>   ***Description***: Change password of account  
>   ***Role Access***: User, Charity, Admin  
>
> - ``PUT /api/play-together/v1/accounts/reset-password-admin ``  
>   ***Description***: Reset password for admin  
>   ***Role Access***: Admin  
>   ***Use for***: Reset password of an Admin account
>
> - ``PUT /api/play-together/v1/accounts/reset-password-token ``  
>   ***Description***: Get token for reset password  
>   ***Role Access***: User, Charity  
>   ***Use for***: Get token for reset password, go with verify email
>
> - ``PUT /api/play-together/v1/accounts/reset-password ``  
>   ***Description***: Reset password  
>   ***Role Access***: User, Charity  
>
> - ``POST /api/play-together/v1/accounts/register-multi-player ``  
>   ***Description***: Register multi Player Account **for TEST**  
>
> - ``POST /api/play-together/v1/accounts/register-multi-user ``  
>   ***Description***: Register multi Normal User Account **for TEST**  
>
> - ``GET /api/play-together/v1/accounts/check-exist-email ``  
>   ***Description***: Check Exist Email
>