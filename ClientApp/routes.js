import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import Directory from 'components/directory'
import Stocks from 'components/stocks'
import CashDesks from 'components/cash-desks'

//import Units from 'components/directory-components/units'
//import Units from 'components/directory-components/units'
export const routes = [
    { path: '/Manage', component: HomePage, display: 'Home', style: '', slot: 'tollbar' },
    {

        path: '/Manage/Directory', component: Directory, display: 'Справочники', style: '', slot: 'tollbar',
        //children: [
        //    { path: '/Products', component: ProductRange, display: 'Номенклатура', style: '', slot: 'directory' },
        //    { path: 'Units', component: Units, display: 'Еденицы измерения', style: '', slot: 'directory' }

        //]

    },
    { path: '/Manage/Stocks', component: Stocks, display: 'Склады', style: '', slot: 'tollbar' },

    { path: '/Manage/CashDesks', component: CashDesks, display: 'Кассы', style: '', slot: 'tollbar' },
    //{ path: '/Manage/fetch-data', component: FetchData, display: 'Fetch data', style: '', slot: 'tollbar' },
    //{ path: '/Manage/Directory/Units', component: Units, display: 'Еденицы измерения', style: '', slot: 'directory' }

]
