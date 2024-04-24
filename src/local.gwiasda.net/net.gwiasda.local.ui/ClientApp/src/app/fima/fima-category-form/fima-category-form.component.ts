import { Component, Output, Input, EventEmitter } from '@angular/core';
import { FinanceCategory } from '../finance_category';
import { FiMaCategorySelectionComponent } from '../fima-category-selection/fima-category-selection.component';
import { FiMaDataService } from '../fima-data.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-fima-category-form',
  templateUrl: './fima-category-form.component.html'
})
export class FiMaCategoryFormComponent {

  constructor(dataService: FiMaDataService, private dialog: MatDialog) { this.dataService = dataService; }

  @Input() costCategories: FinanceCategory[] | undefined;
  @Input() incomeCategories: FinanceCategory[] | undefined;
  @Input() category: FinanceCategory = new FinanceCategory();
  @Output() saved = new EventEmitter<void>();

  selectedParentName: string = '---';
  dataService: FiMaDataService;

  selectParent() {
    const dialogRef = this.dialog.open(FiMaCategorySelectionComponent,
      {
      });

    if (this.category == null)
      this.category = new FinanceCategory();

    if(this.category?.isCostCategory)
      dialogRef.componentInstance.categories = this.costCategories;
    else
      dialogRef.componentInstance.categories = this.incomeCategories;

    dialogRef.componentInstance.isCostCategory = this.category == null ? false : this.category.isCostCategory;
    dialogRef.componentInstance.saved.subscribe((category: FinanceCategory) => {
      if(category.name != null && category.name != '')
        this.selectedParentName = category.name;
      else
        this.selectedParentName = '---';
      if(this.category != null)
        this.category.parentId = category.id;
    });
  }

  async save() {
    this.saved.emit();
    this.selectedParentName = '---';
  }
}
