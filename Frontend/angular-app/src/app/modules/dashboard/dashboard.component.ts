import {Component} from '@angular/core';
import {BaseComponent} from "../../core/abstraction/base.component";
import {Mediator} from "../../core/services/mediator/mediator.service";
import {EChartsOption} from "echarts";
import {IdentityService} from "../identity/repositories/identity.service";
import {
  GetAccountReviewReportQuery
} from "../accounting/repositories/reporting/queries/get-account-review-report-query";
import {
  AccountReviewReportResultModel
} from "../accounting/repositories/account-review/account-review-report-result-model";
import {UserYear} from "../identity/repositories/models/user-year";
import {User} from "../admin/entities/user";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})

export class DashboardComponent extends BaseComponent {

  isBarChartSelected: boolean = true

  pieChart1 = <EChartsOption>{
    textStyle: {
      fontFamily: 'IRANYekanRegular'
    },
    title: {
      text: 'فروش',
      left: 'center'
    },
    tooltip: {
      trigger: 'item',

    },
    legend: {
      bottom: 10,
      left: 'center',
      data: ['فروش نمایندگان', 'صادرات', 'فروش داخلی', 'پیش فروش', 'مصرف کننده']
    },
    series: [
      {
        type: 'pie',
        radius: '65%',
        center: ['50%', '50%'],
        selectedMode: 'single',
        data: [
          {value: 735, name: 'فروش نمایندگان'},
          {value: 510, name: 'صادرات'},
          {value: 434, name: 'فروش داخلی'},
          {value: 335, name: 'پیش فروش'},
          {value: 335, name: 'مصرف کننده'}
        ],
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)'
          }
        }
      }
    ]
  };
  pieChart = <EChartsOption>{
    textStyle: {
      fontFamily: 'IRANYekanRegular'
    },
    title: {
      text: 'مشتریان',
      left: 'center'
    },
    tooltip: {
      trigger: 'item'
    },
    legend: {
      left: 0,
      top: 'center',
      orient: 'vertical',
    },
    series: [
      {
        name: 'تعداد مشتریان',
        type: 'pie',
        radius: ['30%', '70%'],
        center: ['50%', '50%'],

        avoidLabelOverlap: false,

        itemStyle: {
          borderRadius: 10,
          borderColor: '#fff',
          borderWidth: 2
        },
        label: {
          show: false,
          position: 'center'
        },
        emphasis: {
          label: {
            show: true,
            fontSize: '15',
            fontWeight: 'bold'
          }
        },
        labelLine: {
          show: false
        },
        data: [
          {value: 1048, name: 'تهران'},
          {value: 735, name: 'یزد'},
          {value: 580, name: 'مشهد'},
          {value: 484, name: 'تبریز'},
          {value: 300, name: 'اهواز'}
        ]
      }
    ]
  };

  barChart = <EChartsOption>{
    textStyle: {
      fontFamily: 'IRANYekanRegular'
    },
    legend: {},
    tooltip: {},
    dataset: {
      source: [
        ['محصولات', '1398', '1399', '1400', '1401'],
        ['تولید', 41.1, 30.4, 65.1, 53.3],
        ['فروش داخلی', 86.5, 92.1, 85.7, 83.1],
        ['صادرات', 24.1, 67.2, 79.5, 86.4]
      ]
    },
    xAxis: [
      {type: 'category', gridIndex: 0},
      // { type: 'category', gridIndex: 1 }
    ],
    yAxis: [{gridIndex: 0}
      // , { gridIndex: 1 }
    ],
    // grid: [{ bottom: '55%' }, { top: '55%' }],
    series: [
      // These series are in the first grid.
      {type: 'bar', seriesLayoutBy: 'row'},
      {type: 'bar', seriesLayoutBy: 'row'},
      {type: 'bar', seriesLayoutBy: 'row'},
      // These series are in the second grid.
      // { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
      // { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
      // { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
      // { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 }
    ]
  };

  polygonChart = <EChartsOption>{
    textStyle: {
      fontFamily: 'IRANYekanRegular'
    },
    title: {
      text: ''
    },
    legend: {
      data: ['بودجه تخصیص یافته', 'هزینه های واقعی'],
      bottom: 0,
      left: 'center'
    },

    tooltip: {
      trigger: 'item',
    },
    radar: {
      shape: 'polygon',
      indicator: [
        {name: 'فروش', max: 6500},
        {name: 'دفتر مرکزی', max: 16000},
        {name: 'تولید', max: 30000},
        {name: 'حمل نقل', max: 38000},
        {name: 'بازرگانی', max: 52000},
        {name: 'حقوق', max: 25000}
      ]
    },
    series: [
      {
        name: '',
        type: 'radar',

        data: [
          {
            value: [4200, 3000, 20000, 35000, 50000, 18000],
            name: 'بودجه تخصیص یافته',
          },
          {
            value: [5000, 14000, 28000, 26000, 42000, 21000],
            name: 'هزینه های واقعی'
          }
        ]
      }
    ]
  };

  lineChart = <EChartsOption>{
    color: [
      '#EE6666',
      '#5470C6',
    ],
    textStyle: {
      fontFamily: 'IRANYekanRegular'
    },
    legend: {},
    tooltip: {
      trigger: 'item',
    },
    dataset: {
      source: [
        ['product',
          'فروردین 1401',
          'اردیبهشت 1401',
          'خرداد 1401',
          'تیر 1401',
          'مرداد 1401',
          'شهریور 1401',
          'مهر 1401',
          'آبان 1401',
          'آذر 1401',
          'دی 1401',
          'بهمن 1401',
          'اسفند 1401',],
        ['بودجه تخصیص یافته',

          '29',
          '15',
          '12',
          '22',
          '31',
          '49',
          '30',
          '53',
          '67',
          '57',
          '41',
          '24'],
        ['هزینه های واقعی',
          '25',
          '19',
          '10',
          '18',
          '35',
          '40',
          '36',
          '63',
          '70',
          '77',
          '71',
          '54'],
      ]
    },
    xAxis: {type: 'category', boundaryGap: false,},
    yAxis: {},

    series: [
      {
        type: 'line',
        smooth: true,
        seriesLayoutBy: 'row',
        emphasis: {focus: 'series'},
        // areaStyle: {
        //   opacity:'0.5'
        // },
      },
      {
        type: 'line',
        smooth: true,
        seriesLayoutBy: 'row',
        emphasis: {focus: 'series'},
        // areaStyle: {
        //   opacity:'0.5'
        // },
      },

    ]

  };

  revenuesAmount = 0;
  costsAmount = 0;
  costOfAmount = 0;
  grossProfit = 0;
  constructor(
    private _mediator: Mediator,
    public identityService: IdentityService
  ) {
    super();
  }

  async ngOnInit() {
    this.identityService._applicationUser.subscribe(async (user) => {
      if (user.isAuthenticated) {
        await this.getAccountReviewReport()
      }
    })
  }

  async resolve() {
  }

  async initialize(accountHeadIds?: []) {

  }

  async getAccountReviewReport() {
    let query = new GetAccountReviewReportQuery()
    let userCurrentYear = <UserYear>this.identityService.applicationUser.years.find(x => x.isCurrentYear);
    query.accountHeadIds = [];
    query.level = 1;
    query.currencyTypeBaseId =  28306;
    query.yearIds.push(userCurrentYear.id)
    query.voucherDateFrom = userCurrentYear.firstDate
    query.voucherDateTo = userCurrentYear.lastDate
    query.companyId = this.identityService.applicationUser.companyId
    query.reportFormat = 1;

    await this._mediator.send(query).then(res => {
      // @ts-ignore
      this.revenuesAmount = res.find(x => x.code == "6")?.remainCredit
      // @ts-ignore
      this.costsAmount = res.find(x => x.code == "8")?.remainDebit
      // @ts-ignore
      this.costOfAmount = res.find(x => x.code == "7")?.remainDebit
      this.grossProfit = this.revenuesAmount - (this.costOfAmount + this.costsAmount)
    });
  }

  async get(param?: any) {
  }


  add(param?: any): any {
  }

  update(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }
}
