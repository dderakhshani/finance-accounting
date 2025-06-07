import {Component, Input, OnInit} from '@angular/core';
import {EChartsOption} from "echarts";

@Component({
  selector: 'app-bar-widget',
  templateUrl: './bar-widget.component.html',
  styleUrls: ['./bar-widget.component.scss']
})
export class BarWidgetComponent implements OnInit {
  @Input() title:string = '';
  @Input() labels:string[] = []
  @Input() values:number[] = []

  options!:EChartsOption;

  constructor() { }

  ngOnInit(): void {
    this.options =<EChartsOption>{
      responsive: true,
      responsiveAnimationDuration: 300,
      maintainAspectRatio: true,
      scales: {
        xAxes: [{
          display: true,
          ticks: {
            min:0,
            max:50
          }
        }]
      },
      legend: {
        data: [this.title],
        align: 'left',
        textStyle: {
          fontFamily:'IRANYekanRegular'
        }
      },
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'shadow'
        }
      },
      title: {
        show: false,
        textStyle: {
          color: "grey",
          fontSize: 20,
          fontFamily:'IRANYekanRegular'
        },
        text: "No data to display",
        left: "center",
        top: "center"
      },
      grid: {
        bottom: 10,
        top: 35,
        containLabel: true
      },
      yAxis: {
        type: 'value',
        boundaryGap: [0, 0.01]
      },
      xAxis: {
        type: 'category',
        data: this.labels,
        nameTextStyle: {
          fontFamily:'IRANYekanRegular',
          fontSize:15
        },
        show:true,
        axisLabel: {
          fontFamily:'IRANYekanRegular',
          padding: [10,0,0,0],
          fontSize: 10
        }
      },
      series: [
        {
          name: this.title,
          type: "bar",
          smooth: true,
          data: this.values,
          stack: 'total',
          itemStyle: {
            fontFamily: 'IRANYekanRegular',
          },
          label: {
            show: true,
            fontFamily: 'IRANYekanRegular',
            formatter: (param: any) => {
              return param.data == 0 ? '' : param.data;
            }
          },
          emphasis: {
            focus: 'series',
            itemStyle:{
              fontFamily: 'IRANYekanRegular',
            }
          },
          animationDelay: (idx: number) => idx * 10,
        },

      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: (idx: number) => idx * 5,
    };


  }

}

export class BarWidgetOptions {
  values!:any[];
  title!:string;
  labels!:string[]
}
