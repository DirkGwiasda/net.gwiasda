import { CategoryReport } from './categoryReport';
export class MonthlyReport {
  month: Date = new Date();
  costCategoryReports: CategoryReport[] = [];
  incomeCategoryReports: CategoryReport[] = [];
}
