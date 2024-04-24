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
    if (this.categories)
      this.dataSource = new MatTableDataSource<FinanceCategory>(this.categories);
  }

  @Input() categories: FinanceCategory[] | undefined;
  @Input() isCostCategory: boolean = false;;
  @Output() saved = new EventEmitter<FinanceCategory>();

  dataSource: MatTableDataSource<FinanceCategory> = new MatTableDataSource<FinanceCategory>();
  displayedColumns: string[] = ['name'];

  selectCategory(category: FinanceCategory) {
    this.saved.emit(category);
    this.dialogRef.close();
  }
  close() {
    this.dialogRef.close();
  }
}
