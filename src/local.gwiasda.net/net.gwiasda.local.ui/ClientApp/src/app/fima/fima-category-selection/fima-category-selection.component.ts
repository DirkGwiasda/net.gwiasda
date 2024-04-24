import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FinanceCategory } from '../finance_category';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-fima-category-selection',
  templateUrl: './fima-category-selection.component.html'
})
export class FiMaCategorySelectionComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<FiMaCategorySelectionComponent>) { }

  ngOnInit(): void {
    if (this.categories) {
      this.dataSource = new MatTableDataSource<FinanceCategory>(this.categories);
    }
  }

  @Input() categories: FinanceCategory[] | undefined;
  @Input() isCostCategory: boolean = false;;
  @Output() saved = new EventEmitter<FinanceCategory>();

  dataSource: MatTableDataSource<FinanceCategory> = new MatTableDataSource<FinanceCategory>();
  displayedColumns: string[] = ['name'];

  getNameWithSpaces(category: FinanceCategory): string {
    let result = '';
    let count = 0;
    for (let i = 0; i < category.hierarchy; i++) {
      result += '---';
      count++;
    }
    return result + " " + category.name;
  }

  selectCategory(category: FinanceCategory | null) {
    this.saved.emit(category ?? new FinanceCategory());
    this.dialogRef.close();
  }
  close() {
    this.dialogRef.close();
  }
}
