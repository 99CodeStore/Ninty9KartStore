using Newtonsoft.Json;

namespace NintyNineKartStore.Service.Models
{
    public class Error
    {
        public int StatusCode;
        public string Message;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
