using System;
using System.Collections.Generic;
using System.Text;

namespace BladeRazer.TagHelpers
{
    // TODO: Test singleton dependency injecttion
    public class Styles
    {
        public string FormGroup { get; set; } = "form-group";
        public string FormControl { get; set; } = "form-control";
        public string Label { get; set; } = "control-label";
        public string CheckLabel { get; set; } = "form-check-label";
        public string Validation { get; set; } = "text-danger";

        public string DivRow { get; set; } = "row";
        public string DivCol { get; set; } = "col-md-4";

        public string DescriptionList { get; set; } = "row";
        public string DefinitionDescription { get; set; } = "col-sm-2";
        public string DefinitionTerm  { get; set; } = "col-sm-10";

        public string ButtonCancel = "btn btn-primary m-1";
        public string ButtonDelete = "btn btn-danger m-1";
        public string ButtonEdit = "btn btn-info m-1";
        public string ButtonNew = "btn btn-success m-1";
        public string ButtonView = "btn btn-primary m-1";

        public string Table = "table table-hover table-responsive w-100 d-block d-md-table";
        public string TableCellHideMobile =  "d-none d-sm-table-cell";
    }
}
