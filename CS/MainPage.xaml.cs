using DevExpress.Maui.DataGrid;
using DevExpress.Maui.Core;

namespace maui_grid_get_started_migrated {
    public partial class MainPage : ContentPage {
        int count;

        public MainPage() {
            InitializeComponent();
        }

        void grid_CalculateCustomSummary(System.Object sender, DevExpress.Maui.DataGrid.CustomSummaryEventArgs e) {
            if (e.FieldName.ToString() == "Shipped")
                if (e.IsTotalSummary) {
                    if (e.SummaryProcess == DataSummaryProcess.Start) {
                        count = 0;
                    }
                    if (e.SummaryProcess == DataSummaryProcess.Calculate) {
                        if (!(bool)e.FieldValue)
                            count++;
                        e.TotalValue = count;
                    }
                }
        }
    }
}