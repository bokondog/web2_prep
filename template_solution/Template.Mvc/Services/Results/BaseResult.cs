namespace Template.Mvc.Services.Results
{
    public abstract class BaseResult
    {
        private bool _succeeded = false;
        public bool Succeeded => _succeeded;
        public string? Error { get; set; }
        public void Failed(string error)
        {
            _succeeded = false;
            Error = error;
        }
        public void Success()
        {
            _succeeded = true;
        }
    }
}
