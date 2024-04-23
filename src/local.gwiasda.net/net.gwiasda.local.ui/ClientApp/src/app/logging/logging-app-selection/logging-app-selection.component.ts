import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { LoggingDataService } from '../logging-data.service';

@Component({
  selector: 'app-logging-app-selection',
  templateUrl: './logging-app-selection.component.html',
})
export class LoggingAppSelectionComponent implements OnInit {

  @Output() selectedChange = new EventEmitter<string>();
  @Output() deleteLog = new EventEmitter<void>();

  constructor(private dataService: LoggingDataService) { }

  appNames: Array<string> = [];

  ngOnInit() {
    this.readAppNames();
  }

  appSelected(eventTarget: any) {
    const t = eventTarget as HTMLTextAreaElement;
    if (!t) {
      console.log("selectApp: eventTarget as HTMLTextAreaElement is null");
      return;
    }
    const appName = eventTarget.value as string;
    this.selectApp(appName);
  }
  selectApp(appName: string) {
    this.selectedChange.emit(appName);
  }
  delete() {
    this.deleteLog.emit();
  }
  async readAppNames(): Promise<void> {
    this.dataService.readAppNames()
      .then(appNames => {
        this.appNames = appNames ?? [];
        if (this.appNames.length > 0)
          this.selectApp(this.appNames[0]);
      });
  }
}
