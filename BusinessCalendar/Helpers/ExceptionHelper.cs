using System.Text;

namespace BusinessCalendar.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetFullMessage(Exception e)
        {
            var message = new StringBuilder(e.Message);

            var innerEx = e.InnerException;

            while (innerEx != null)
            {
                message.AppendLine(innerEx.Message);
                innerEx = innerEx.InnerException;
            }

            return message.ToString();
        }

    }
}
