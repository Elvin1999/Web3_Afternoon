using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using Web3_Afternoon.Dtos;

namespace Web3_Afternoon.Formatters
{
    public class CarVCardOutputFormatter : TextOutputFormatter
    {

        public CarVCardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        
        protected override bool CanWriteType(Type? type)
        {
            if(typeof(CarExtendDto).IsAssignableFrom(type))
                return true;

            if (typeof(IEnumerable<CarExtendDto>).IsAssignableFrom(type))
                return true;

            return false;
        }

        private static Task WriteVCard(CarExtendDto car,HttpResponse response)
        {
            return response.WriteAsync($@"
BEGIN:VCARD
VERSION:3.0
MV:{car.Model} {car.Vendor}
NOTE:Year = {car.Year}
NOTE:Engine = {car.Engine}
END:VCARD
");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response=context.HttpContext.Response;

            if (context.Object is IEnumerable<CarExtendDto> cars)
            {
                foreach (var car in cars)
                {
                    await WriteVCard(car, response);
                }
            }
            else
            {
                await WriteVCard((CarExtendDto)context.Object, response);
            }
        }
    }
}
