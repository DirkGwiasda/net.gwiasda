import { Component } from '@angular/core';
import { LoggingDataService } from './logging-data.service';
import { LogEntry } from './logentry';

@Component({
  selector: 'app-logging',
  templateUrl: './logging.component.html',
  styleUrls: ['./logging.component.css'],
})
export class LoggingComponent {

  constructor(private dataService: LoggingDataService) { }

  selectedLogAppName: string = '';

  async selectLogAppChanged(newLogAppName: string): Promise<void> {
    if (!newLogAppName) return;
    
    this.selectedLogAppName = newLogAppName;
  }
  async deleteLogs(): Promise<void> {
    if (!this.selectedLogAppName) return;
    await this.dataService.deleteEntries(this.selectedLogAppName);
    window.location.reload();
  }
}
