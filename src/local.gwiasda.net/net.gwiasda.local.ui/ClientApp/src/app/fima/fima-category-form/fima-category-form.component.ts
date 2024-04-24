import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FinanceCategory } from '../finance_category';
import { FiMaCategorySelectionComponent } from '../fima-category-selection/fima-category-selection.component';
import { FiMaDataService } from '../fima-data.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-fima-category-form',
  templateUrl: './fima-category-form.component.html'
})
export class FiMaCategoryFormComponent implements OnChanges {

  constructor(dataService: FiMaDataService, private dialog: MatDialog) { this.dataService = dataService; }

  @Input() costCategories: FinanceCategory[] | undefined;
  @Input() incomeCategories: FinanceCategory[] | undefined;
  @Input() category: FinanceCategory = new FinanceCategory();
  @Output() saved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<void>();

  selectedParentName: string = '---';
  dataService: FiMaDataService;

  async ngOnChanges(changes: SimpleChanges): Promise<void> {
    if (this.category.parentId != null && this.category.parentId != '') {
      let parentCategory: FinanceCategory | null = null;
      if (this.category.isCostCategory)  
        this.setParentCategoryName(this.costCategories, this.category);
      else
        this.setParentCategoryName(this.incomeCategories, this.category);
    }
    else
      this.selectedParentName = '---';
  }
  setParentCategoryName(categories: FinanceCategory[] | undefined, category: FinanceCategory) {
    const items = categories?.filter(c => c.id == this.category.parentId);
    let parentCategory = items?.length == 1 ? items[0] : null;
    if (parentCategory)
      this.selectedParentName = parentCategory.name;
    else
      this.selectedParentName = '---';
  }

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
      dialogRef.componentInstance.allowNoneSelection = true;
    
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
  moveDown() {
    this.category.position++;
    this.saved.emit();
  }
  moveUp() {
    if (this.category.position == 0) return;
    this.category.position--;
    this.saved.emit();
  }
  async save() {
    this.saved.emit();
  }
  async cancel() {
    this.cancelled.emit();
  }
  async delete() {
    this.deleted.emit();
  }
}
