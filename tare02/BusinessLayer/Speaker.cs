using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? Experience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public int RegistrationFee { get; set; }
		public List<BusinessLayer.Session> Sessions { get; set; }

        //variables Globales
        const int NivelOrador1 = 1;
        const int NivelOrador2= 2;
        const int NivelOrador3 =3;
        const int NivelOrador4 = 4;
        const int NivelOrador5 = 5;
        const int NivelOrador6 = 6;
        const int NivelOrador9 = 9;
        const int NivelOrador10 = 10;

        const int MontoCuota0 = 0;
        const int MontoCuota50 = 50;
        const int MontoCuota100 = 100;
        const int MontoCuota250 = 250;
        const int MontoCuota500 = 500;
        const int numberCertificaction = 3;
        const int VersionMin = 9;


        public int? Register(IRepository repository)
		{

            int? speakerId = null;
			var ListOt = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
			var ListDomainsFilter = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };

            try
            {

                if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentNullException("First Name is required");
                if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentNullException("Last name is required.");
                if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException("Email is required.");
                if (!meetsStandards()) throw new SpeakerDoesntMeetRequirementsException("Speaker doesn't meet our abitrary and capricious standards.");
                if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");
                if (!sessionsApproved()) throw new NoSessionsApprovedException("No sessions approved.");

                RegistrationFee = calculateRecordFee();



                speakerId = repository.SaveSpeaker(this);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error: " + e.Message);
            }

            return speakerId;
        }
    
            public bool meetsStandards()
            {
                var ListDomains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };
                var ListEmployers = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };
                if ((Experience > NivelOrador10 || HasBlog || Certifications.Count() > 3 || ListEmployers.Contains(Employer)))
                    return true;
                else
                {
                    string emailDomain = Email.Split('@').Last();
                    return !(ListDomains.Contains(emailDomain) && ((Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < VersionMin)));
                }
            }

        public bool sessionsApproved()
        {
            bool approved = false;
            var ListOldTecnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };

            foreach (var session in Sessions)
            {
                foreach (var tech in ListOldTecnologies)
                {
                    session.Approved = !(session.Title.Contains(tech) || session.Description.Contains(tech));
                    if (session.Approved) approved = true;
                }
            }
            return approved;
        }
        public int calculateRecordFee()
        {
            int RegistrationFee = MontoCuota0;
            switch (Experience)
            {
                case NivelOrador1:
                    RegistrationFee = MontoCuota500;
                    break;
                case NivelOrador2:
                case NivelOrador3:
                    RegistrationFee = MontoCuota250;
                    break;
                case NivelOrador4:
                case NivelOrador5:
                    RegistrationFee = MontoCuota100;
                    break;
                case NivelOrador6:
                case NivelOrador9:
                case NivelOrador10:
                    RegistrationFee = MontoCuota50;
                    break;
            }
            return RegistrationFee;
        }


        #region Custom Exceptions
        public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message)
				: base(message)
			{
			}

			public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
				: base(string.Format(format, args)) { }
		}

		public class NoSessionsApprovedException : Exception
		{
			public NoSessionsApprovedException(string message)
				: base(message)
			{
			}
		}
		#endregion
	}
}