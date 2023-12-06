//namespace LotteriaView
//{
//    public class Traisaction
//    {

//        private readonly RequestDelegate _next;

//        public MiddleExp(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            using (var scope = context.RequestServices.CreateScope())
//            {
//                var dbContext = scope.ServiceProvider.GetRequiredService<>();

//                await using (var transaction = await dbContext.Database.BeginTransactionAsync())
//                {
//                    try
//                    {
//                        await _next(context);
//                        await transaction.CommitAsync();
//                    }
//                    catch
//                    {
//                        await transaction.RollbackAsync();
//                        throw;
//                    }
//                }
//            }
//        }
//    }
//}



