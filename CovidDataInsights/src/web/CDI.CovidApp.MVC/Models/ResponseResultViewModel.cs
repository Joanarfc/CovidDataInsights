namespace CDI.CovidApp.MVC.Models
{
    public class ResponseResultViewModel
    {
        public ResponseResultViewModel()
        {
            Errors = new ResponseErrorMessages();
        }
        public string? Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages Errors { get; set; }
    }
    public class ResponseErrorMessages
    {
        public ResponseErrorMessages()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }
    }
}