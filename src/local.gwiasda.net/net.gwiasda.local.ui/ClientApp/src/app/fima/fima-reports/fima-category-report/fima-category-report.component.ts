import { Component, Input } from '@angular/core';
import { CategoryReport } from '../categoryReport';
import { MatDialog } from '@angular/material/dialog';
import { FiMaCategoryReportBookingsComponent } from '../fima-category-report-bookings/fima-category-report-bookings.component';

@Component({
  selector: 'app-fima-category-report',
  templateUrl: './fima-category-report.component.html'
})
export class FiMaCategoryReportComponent {

  constructor(private dialog: MatDialog) { }

  @Input() categoryReport: CategoryReport | undefined;

  getSpaceWidth(add: number): string {
    if(!this.categoryReport) {
      return 'width: 0px;';
    };

    return `width: ${(this.categoryReport.category.hierarchy + add) * 10}px;`;
  }
  getTextWidth(add: number): string {
    if (!this.categoryReport) {
      return 'width: 0px;';
    };

    return `width: ${400 - (this.categoryReport.category.hierarchy + add) * 90}px;`;
  }
  getBookingsWidth(add: number): string {
    if (!this.categoryReport) {
      return 'width: 0px;';
    };

    return `width: ${400 - (this.categoryReport.category.hierarchy + add) * 80}px;`;
  }
  showBookings() {

    const dialogRef = this.dialog.open(FiMaCategoryReportBookingsComponent, { });

    if (this.categoryReport == null) return;

    dialogRef.componentInstance.categoryReport = this.categoryReport;
  }
}
