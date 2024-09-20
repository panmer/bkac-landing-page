using gcsharpRPC.Models;
using gcsharpRPC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace gcsharpRPC.Pages.Polls
{
    public class CreatePollPageModel : PageModel
    {
        private readonly PollService _service;

        private readonly ILogger _logger;

        [BindProperty]
        public Poll Poll { get; set; }

        [BindProperty]
        public DateTime[] PollOptionDates { get; set; }

        [BindProperty]
        public string[] PollOptionStartTimes { get; set; }

        [BindProperty]
        public string[] PollOptionEndTimes { get; set; }
        
        public CreatePollPageModel(PollService pollService,
                            ILogger<CreatePollPageModel> logger)
        {
            _service = pollService;
            _logger = logger;
        }

        public IActionResult OnGet() {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            // if (HttpContext.Session.GetString("username") is null)
            // {
            //     return Redirect("/Login");
            // }

            _logger.LogInformation("Called OnPostAsync!!");

            if (ModelState.IsValid) {
                _logger.LogInformation("Model Valid");
            }

            if (Poll is not null) {
                _logger.LogInformation("Poll is not null");
            }

            // if (PollOptions is not null) {
            //     _logger.LogInformation("PollOptions is not null");
            //     // _logger.LogInformation(PollOptions.StartTime);
            // }

            if (PollOptionDates.Length == 0)
            {
                // _logger.LogInformation("PollOptions is zero");
                ViewData["PollOptionErrors"] = "Poll Option Cannot Be Empty!";
            }

            PollOption[] PollOptions = new PollOption[PollOptionDates.Length];

            for (int i=0; i<PollOptionEndTimes.Length; ++i)
            {
                // _logger.LogInformation("--> " + PollOptionDates[i]);
                // _logger.LogInformation("--> " + PollOptionStartTimes[i]);
                // _logger.LogInformation("--> " + PollOptionEndTimes[i]);

                PollOptions[i] = new PollOption {
                    Date = PollOptionDates[i],
                    StartTime = PollOptionStartTimes[i],
                    EndTime = PollOptionEndTimes[i]
                };
            }

            if (ViewData["PollOptionErrors"] is null)
            {
                return Page();
            }

            if (ModelState.IsValid && Poll is not null) {
                await _service.CreatePollAsync(Poll, PollOptions);
                _logger.LogInformation(Poll.ToString());
                return Page();
            }

            return Page();
        }
    }
}