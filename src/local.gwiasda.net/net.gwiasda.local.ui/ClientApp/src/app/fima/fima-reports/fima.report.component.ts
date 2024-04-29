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

  ngOnInit() {
    this.readMonthlyReport(new Date());
  }

  readMonthlyReport(date: Date) {
    console.log("now calling fimareport be..." + new Date());
    this.dataService.readMonthlyReportForDate(date).subscribe(
      report => {
        this.monthlyReport = report;
        console.log(new Date());
        console.log(this.monthlyReport);
      });
  }
}
