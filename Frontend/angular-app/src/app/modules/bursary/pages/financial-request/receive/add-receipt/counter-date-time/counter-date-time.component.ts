import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { interval, BehaviorSubject, Subscription } from 'rxjs';

@Component({
  selector: 'app-counter-date-time',
  template: `
    <div class="timer">
      <span class="timer-border" *ngIf="!expired"> ویرایش : {{ days }}:{{ hours }}:{{ minutes }}:{{ seconds }} </span>
      <span class="expire-border" title="پس از اتما زمان ویرایش شما صرفا درخواست ویرایش را برای حسابرس مربوطه ارسال میکنید و با تایید ایشان ویرایش شما انجام خواهد شد" *ngIf="expired">اتمام زمان ویرایش</span>
    </div>
  `,
  styles: [`
    .timer {
      display: flex;
      justify-content: center;
      align-items: center;
      font-size: 2rem;
    }
    span:not(:last-child) {
      margin-right: 1rem;
    }
    span:nth-child(2) {
      color: red;
    }
    .timer-border {
      font-size: small;
    background: orange;
    padding: 4px;
    border-radius: 8px;
    color: white;
    }

    .expire-border {
      font-size: small;
    background: red;
    padding: 4px;
    border-radius: 8px;
    color: white;
    }

  `]
})
export class CounterDateTimeComponent implements OnInit, OnDestroy {
  @Input() startTime: string = '';

  private timerSubscription: Subscription | undefined;

  ngOnInit() {
   // const serverTime = new Date(this.startTime).getTime();
    const futureTime = new Date(new Date(this.startTime).getTime() + (72 * 60 * 60 * 1000)).getTime();

    this.timerSubscription = interval(1000).subscribe(() => {
      const now = Date.now();
      const timeDiff = futureTime - now;
      if (timeDiff <= 0) {
        this.expired$.next(true);
        this.unsubscribeTimer();
      } else {
        const seconds = Math.floor((timeDiff / 1000) % 60);
        const minutes = Math.floor((timeDiff / (1000 * 60)) % 60);
        const hours = Math.floor((timeDiff / (1000 * 60 * 60)) % 24);
        const days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));

        const pad = (num: number): string => (num < 10 ? '0' : '') + num;

        const timeLeft = `${days}:${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
        this.timeLeft$.next(timeLeft);
      }
    });
  }

  ngOnDestroy() {
    this.unsubscribeTimer();
  }

  timeLeft$ = new BehaviorSubject<string>('');
  expired$ = new BehaviorSubject<boolean>(false);

  get days(): number {
    return Math.max(Math.floor((new Date(new Date(this.startTime).getTime() + (72 * 60 * 60 * 1000)).getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24)), 0);
  }

  get hours(): number {
    return Math.max(Math.floor((new Date(new Date(this.startTime).getTime() + (72 * 60 * 60 * 1000)).getTime() - new Date().getTime()) / (1000 * 60 * 60) % 24), 0);
  }

  get minutes(): number {
    return Math.max(Math.floor((new Date(new Date(this.startTime).getTime() + (72 * 60 * 60 * 1000)).getTime() - new Date().getTime()) / (1000 * 60) % 60), 0);
  }

  get seconds(): number {
    return Math.max(Math.floor((new Date(new Date(this.startTime).getTime() + (72 * 60 * 60 * 1000)).getTime() - new Date().getTime()) / 1000 % 60), 0);
  }

  get expired(): boolean {
    return this.expired$.getValue();
  }

  private unsubscribeTimer(): void {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
      this.timerSubscription = undefined;
    }
  }
}
