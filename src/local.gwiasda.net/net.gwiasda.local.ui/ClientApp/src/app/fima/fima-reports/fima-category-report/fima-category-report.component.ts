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
  getName() {
    if (!this.categoryReport?.category) {
      return '????';
    }

    if ((!this.categoryReport?.childCategories || this.categoryReport.childCategories.length === 0)
      || (!this.categoryReport?.bookings || this.categoryReport.bookings.length === 0))
      return this.categoryReport.category.name;

    var sumBookings = 0;
    for (var i = 0; i < this.categoryReport.bookings.length; i++) {
      sumBookings += this.categoryReport.bookings[i].amount;
    }

    const options: Intl.NumberFormatOptions = {
      style: 'decimal',
      useGrouping: true,
      minimumFractionDigits: 2, // Mindestanzahl an Nachkommastellen
      maximumFractionDigits: 2, // Maximale Anzahl an Nachkommastellen
    };

    // Formatierung mit den deutschen Optionen
    const formattedNumber: string = sumBookings.toLocaleString('de-DE', options);


    return this.categoryReport.category.name + ' (' + formattedNumber + ' €)';
  }
}
