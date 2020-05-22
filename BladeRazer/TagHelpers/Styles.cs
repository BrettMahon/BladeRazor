using System;
using System.Collections.Generic;
using System.Text;

namespace BladeRazor.TagHelpers
{
    public interface IStyles
    {
        string FormGroup { get; set; }
        string FormControl { get; set; }
        string FormCheck { get; set; }
        
        string Label { get; set; }
        string CheckLabel { get; set; }
        string Validation { get; set; }

        string DivRow { get; set; }
        string DivCol { get; set; }

        string DescriptionList { get; set; }
        string DefinitionDescription { get; set; }
        string DefinitionTerm { get; set; }

        string ButtonSubmit { get; set; }
        string ButtonCancel { get; set; }
        
        string ButtonNew { get; set; }
        string ButtonNewIcon { get; set; }
        
        string ButtonEdit { get; set; }
        string ButtonView { get; set; }
        string ButtonDelete { get; set; }

        string Table { get; set; }
        string TableCellHideMobile { get; set; }
    }

    public class Styles : IStyles
    {
        public string FormGroup { get; set; } = "form-group";
        public string FormControl { get; set; } = "form-control";
        public string FormCheck { get; set; } = "form-check pb-1 pt-2";

        public string Label { get; set; } = "control-label";
        public string CheckLabel { get; set; } = "form-check-label";
        public string Validation { get; set; } = "text-danger";

        public string DivRow { get; set; } = "row";
        public string DivCol { get; set; } = "col-md-4";

        public string DescriptionList { get; set; } = "row";
        public string DefinitionDescription { get; set; } = "col-sm-2";
        public string DefinitionTerm { get; set; } = "col-sm-10";

        public string ButtonSubmit { get; set; } = "btn btn-info m-1";
        public string ButtonCancel { get; set; } = "btn btn-primary m-1";

        public string ButtonNew { get; set; } = "btn btn-success mt-2 mb-2";
        public string ButtonNewIcon { get; set; } = "oi oi-plus";        
        
        public string ButtonEdit { get; set; } = "btn btn-sm btn-info mr-1 mb-1";        
        public string ButtonView { get; set; } = "btn btn-sm btn-primary mr-1 mb-1";
        public string ButtonDelete { get; set; } = "btn btn-sm btn-danger mr-1 mb-1";

        public string Table { get; set; } = "table table-hover table-responsive w-100 d-block d-md-table";
        public string TableCellHideMobile { get; set; } = "d-none d-sm-table-cell";
    }
}
