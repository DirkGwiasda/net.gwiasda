import { Component, Output, EventEmitter } from '@angular/core';
import { FinanceCategory } from '../finance_category';
import { FiMaDataService } from '../fima-data.service';

@Component({
  selector: 'app-fima-form',
  templateUrl: './fima-form.component.html'
})
export class FiMaFormComponent {

  constructor(dataService: FiMaDataService) { this.dataService = dataService; }

  @Output() saved = new EventEmitter<void>();

  dataService: FiMaDataService;
  category: FinanceCategory = new FinanceCategory();

  async save() {

    //await this.dataService.write(this.appointment);
    this.saved.emit();
  }
}
