import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { LogEntry } from '../../logentry';
import { LogEntryAttribute } from '../../logentry-attribute';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-logentry-details',
  templateUrl: './logentry-details.component.html',
  styleUrls: ['./logentry-details.component.css'],
})
export class LogEntryDetailsComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<LogEntryDetailsComponent>) { }
    ngOnInit(): void {
      if (this.logEntry?.attributes)
        this.dataSource = new MatTableDataSource<LogEntryAttribute>(this.logEntry.attributes);
    }

  @Input() logEntry: LogEntry | undefined;

  dataSource: MatTableDataSource<LogEntryAttribute> = new MatTableDataSource<LogEntryAttribute>();

  displayedColumns: string[] = ['name', 'value'];

  close() {
    this.dialogRef.close();
  }

}
