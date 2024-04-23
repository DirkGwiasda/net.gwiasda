import { LogEntryAttribute } from './logentry-attribute';

export class LogEntry {
  id: string = '';
  date: string = '';
  timestamp: string = '';
  logType: string = '';
  text: string = '';
  attributes: LogEntryAttribute[] = [];
}
