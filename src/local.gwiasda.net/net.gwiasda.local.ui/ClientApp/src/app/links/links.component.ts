import { Component, OnInit } from '@angular/core';
import { LinkDataService } from './link-data.service';
import { LinkFormComponent } from './link-form/link-form.component';
import { Link } from './link';
import { GuidService } from '../guid-service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-links',
  templateUrl: './links.component.html',
})
export class LinksComponent implements OnInit {
  constructor(private dataService: LinkDataService, private guidService: GuidService, private dialog: MatDialog) { }

  links: Link[] = [];

  ngOnInit() {
    this.readLinks();
  }
  readLinks() {
    this.dataService.read().subscribe(links => {
      if (links)
        this.links = links;
    });
  }
  editLink(link: Link) {
    this.openForm(link);
  }
  addLink() {
    var link = new Link();
    link.id = this.guidService.generateGUID();
    this.openForm(link);
  }
  async deleteLink(link: Link) {
    await this.dataService.delete(link).then(() => { this.readLinks() });
  }
  async save(link: Link) {
    await this.dataService.write(link).then(() => { this.readLinks() });
  }

  async openForm(link: Link) {
    const dialogRef = this.dialog.open(LinkFormComponent, {});

    dialogRef.componentInstance.link = link;
    dialogRef.componentInstance.saved.subscribe((link: Link) => {
      if (link?.id != null) {
        this.save(link);
      }
    });
  }
}
