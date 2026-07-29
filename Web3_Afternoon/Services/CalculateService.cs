namespace Web3_Afternoon.Services
{
    public class CalculateService : ICalculateService
    {
        int value = 100;
        public CalculateService()
        {
            Console.WriteLine("Service created new object");
        }
        public int Calculate(int num1, int num2)
        {
            value += 100;
            return value;
        }
    }
}
