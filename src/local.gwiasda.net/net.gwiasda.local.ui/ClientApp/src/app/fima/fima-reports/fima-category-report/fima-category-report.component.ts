import { Component, Input } from '@angular/core';
import { CategoryReport } from '../categoryReport';

@Component({
  selector: 'app-fima-category-report',
  templateUrl: './fima-category-report.component.html'
})
export class FiMaCategoryReportComponent {

  @Input() categoryReport: CategoryReport | undefined;

}
