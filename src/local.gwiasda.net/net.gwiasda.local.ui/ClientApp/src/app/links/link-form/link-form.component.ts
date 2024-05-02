import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { Link } from '../link';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './link-form.component.html',
})
export class LinkFormComponent implements OnInit {
  constructor(private dialogRef: MatDialogRef<LinkFormComponent>) { }

  @Input() link: Link = new Link();
  @Output() saved = new EventEmitter<Link>();

  ngOnInit() {
  }

  async save() {
    this.saved.emit(this.link);
    this.cancel();
  }
  cancel() {
    this.dialogRef.close();
  }
}
