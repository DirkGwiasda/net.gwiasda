import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DateService } from '../../date-service';

@Component({
  selector: 'app-appointments-timespan-selection',
  templateUrl: './appointments-timespan-selection.component.html'
})
export class AppointmentsTimeSpanSelectionComponent {

  constructor(private dateService: DateService) { }

  @Input() from: Date = new Date();
  @Input() to: Date = new Date();
  @Output() selected: EventEmitter<Date[]> = new EventEmitter<Date[]>();

  ngOnInit() {
  }

  async refresh() {
    this.selected.emit([this.from, this.to]);
  }
}
