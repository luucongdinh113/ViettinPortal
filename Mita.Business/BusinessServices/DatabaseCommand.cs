namespace Mita.Business.BusinessServices
{
    public class DatabaseCommand<InVal, ReturnVal>
    {
        public InVal CallingInfo { get; set; }
        public ReturnVal ReturnValue { get; set; }
    }
}
