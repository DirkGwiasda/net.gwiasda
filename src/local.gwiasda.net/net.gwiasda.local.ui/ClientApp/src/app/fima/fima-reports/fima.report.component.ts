import { Component, OnInit } from '@angular/core';
import { FiMaReportsDataService } from './fima-reports-data.service';
import { MonthlyReport } from './monthlyReport';

@Component({
  selector: 'app-fima-report',
  templateUrl: './fima.report.component.html'
})
export class FiMaReportComponent implements OnInit {

  constructor(private dataService: FiMaReportsDataService) { }

  monthlyReport: MonthlyReport = new MonthlyReport();
  dateDisplay: string = '';
  date: Date = new Date();
  isLoading: boolean = false;

  ngOnInit() {
    this.readMonthlyReport(this.date);
  }

  readMonthlyReport(date: Date) {
    this.isLoading = true;
    this.dataService.readMonthlyReportForDate(date).subscribe(
      report => {
        this.monthlyReport = report;
        this.isLoading = false;
      });
  }
  closeDatePicker(eventData: any, dp?: any) {

    this.date = new Date(eventData);
    dp.close();
    this.readMonthlyReport(this.date);
  }
}
