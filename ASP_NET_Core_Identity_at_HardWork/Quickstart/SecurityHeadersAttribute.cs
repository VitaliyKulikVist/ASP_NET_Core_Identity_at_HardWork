using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServerHost.Quickstart.UI
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");///Блокує запит, якщо призначення запиту має стиль типу, а тип MIME не є text/css, або має тип script, а тип MIME не є типом <seealso cref="MIME JavaScript"> https://html.spec.whatwg.org/multipage/infrastructure.html#javascript-mime-type</seealso>  
                }
                
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");//Сторінку можна відобразити, лише якщо всі вихідні кадри мають те саме походження, що й сама сторінка.
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";

                // також подумайте про додавання upgrade-insecure-requests, коли у вас буде HTTPS для виробництва
                //csp += "upgrade-insecure-requests;";
                // також приклад, якщо вам потрібно, щоб зображення клієнтів відображалися наприклад з twitter
                // csp += "img-src 'self' https://pbs.twimg.com;";

                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
                }

                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
                }

                var referrer_policy = "no-referrer"; 

                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
                }
            }
        }
    }
}
