import { Component, OnChanges, Input, SimpleChanges } from '@angular/core';
import { LogEntry } from '../logentry';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { LoggingDataService } from '../logging-data.service';
import { LogEntryDetailsComponent } from './logentry-details/logentry-details.component';

@Component({
  selector: 'app-logging-entries',
  templateUrl: './logging-entries.component.html',
  styleUrls: ['./logging-entries.component.css']
})
export class LoggingEntriesComponent implements OnChanges {

  constructor(private dataService: LoggingDataService, private dialog: MatDialog) { }

  async ngOnChanges(changes: SimpleChanges): Promise<void> {
    await this.readLogEntries().then(() =>
      this.dataSource = new MatTableDataSource<LogEntry>(this.logEntries)
      );
  }

  @Input() appName: string | undefined;

  logEntries: LogEntry[] = [];
  dataSource: MatTableDataSource<LogEntry> = new MatTableDataSource<LogEntry>(this.logEntries);
  displayedColumns: string[] = ['type', 'timestamp', 'attributes'];

  async readLogEntries(): Promise<void> {
    if (!this.appName) return;

    await this.dataService.readLogEntries(this.appName)
      .then(entries => {
        if (entries)
          this.logEntries = entries;
      });
  }

  showEntry(entry: LogEntry) {
    const dialogRef = this.dialog.open(LogEntryDetailsComponent,
      {
      });
    dialogRef.componentInstance.logEntry = entry;
  }

  switchAttributes(id: string) {
    var el = document.getElementById(id);
    if (el == null) return;

    if (el.style.display === 'none')
      el.style.display = '';
    else
      el.style.display = 'none';
  }
}
