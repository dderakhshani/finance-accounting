import {Component, Input, OnInit} from '@angular/core';
import {EChartsOption} from "echarts";

@Component({
  selector: 'app-gauge-widget',
  templateUrl: './gauge-widget.component.html',
  styleUrls: ['./gauge-widget.component.scss']
})
export class GaugeWidgetComponent implements OnInit {

  options!: EChartsOption;

  colors = [
    [0.25, '#F9F871'],
    [0.50, '#FAAE5A'],
    [0.75, '#F3AAB3'],
    [1, '#73D776']
  ]
  invertedColors = [
    [0.25, '#F9F871'],
    [0.50, '#FAAE5A'],
    [0.75, '#F3AAB3'],
    [1, '#73D776']
  ]


  _title: string = '';
  @Input() set title(value: string) {
    this._title = value;
    this.ngOnInit()
  }

  get title() {
    return this._title;
  }

  _value: any = 50;
  @Input() set value(value: any) {
    this._value = value;
    this.ngOnInit()
  }

  get value() {
    return this._value;
  }

  _min: number = 0;
  @Input() set min(value: number) {
    this._min = value;
    this.ngOnInit()
  }

  get min() {
    return this._min;
  }

  _max: number = 100;
  @Input() set max(value: number) {
    this._max = value;
    this.ngOnInit()
  }

  get max() {
    return this._max;
  }

  _split: number = 5;
  @Input() set split(value: number) {
    this._split = value;
    this.ngOnInit()
  }

  get split() {
    return this._split;
  }

  _unitTitle: string = '';
  @Input() set unitTitle(value: string) {
    this._unitTitle = value;
    this.ngOnInit()
  }

  get unitTitle() {
    return this._unitTitle;
  }


  @Input() colorDirection: 'rtl' | 'ltr' = 'ltr';
  @Input() unit: number = 1;

  constructor() {
  }

  ngOnInit(): void {
    let that = this;
    this.options = <EChartsOption>{
      series: [
        {

          type: 'gauge',
          startAngle: 230,
          endAngle: -50,
          min: Math.floor(this.min / this.unit),
          max: Math.ceil(this.max / this.unit),
          splitNumber: this.split,
          itemStyle: {
            color: 'rgba(255,0,0,0.3)',
            shadowColor: 'rgba(0,138,255,0.45)',
            shadowBlur: 10,
            shadowOffsetX: 2,
            shadowOffsetY: 2
          },
          progress: {
            show: false,
            roundCap: false,
            width: 0
          },

          axisLine: {
            roundCap: false,
            lineStyle: {
              width: 15,
              color: this.colorDirection === 'ltr' ? this.colors : this.invertedColors
            }
          },
          axisTick: {
            splitNumber: 6,
            lineStyle: {
              width: 2,
              color: '#999'
            }
          },
          splitLine: {
            length: 10,
            lineStyle: {
              width: 3,
              color: '#999'
            }
          },
          axisLabel: {
            distance: 30,
            color: '#999',
            fontSize: '12px',
            formatter: function (value) {
              return value ? value.toFixed(0) : 0;
            },
          },

          pointer: {
            icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
            length: '75%',
            width: 12,
            offsetCenter: [0, '5%']
          },
          title: {
            show: true,
            fontFamily: 'IRANYekanRegular',
            offsetCenter: [0, '-115%'],
            fontSize: 20
          },
          detail: {
            backgroundColor: '#fff',
            borderColor: '#999',
            borderWidth: 2,
            width: '70%',
            lineHeight: 25,
            height: 20,
            borderRadius: 1,
            offsetCenter: [0, '80%'],
            valueAnimation: true,
            formatter: function (value) {
              return '{value|' + (value ? value.toFixed(2) : '') + '} \n {unit|' + `${that.unitTitle} }`;
            },
            rich: {
              value: {
                fontSize: 15,
                fontWeight: 'bolder',
                color: '#777',
                fontFamily: 'IRANYekanRegular',
                padding: [0, 0, 0, 10],

              },
              unit: {
                fontSize: 15,
                color: '#999',
                padding: [5, 12, 0, 0],
                fontFamily: 'IRANYekanRegular',
              }
            }
          },
          data: [
            {
              value: (this.value) / this.unit,
              name: this.title
            }
          ]
        }
      ]
    };
  }

}


export class GaugeWidgetOptions {
  public value!:any[]
  public title!:string
  public unitTitle!:string
  public min!:number
  public max!:number
  public unit!:number
  public colorDirection:'ltr' | 'rtl' = 'ltr'
}
