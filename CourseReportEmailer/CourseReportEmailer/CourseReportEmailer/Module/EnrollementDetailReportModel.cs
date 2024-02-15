using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseReportEmailer.Module
{
    class EnrollementDetailReportModel
    {
        public int EnrollementId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CourseCode { get; set; }
        public string? Desription {  get; set; }
    }
}
