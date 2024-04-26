export class Booking {
  id: string = '';
  timestamp: Date = new Date();
  text: string = '';
  categoryId: string = '';
  isCost: boolean = false;
  amount: number = 0;
  recurringType: string = 'einmalig';
  endDate: Date | null = null;
}
