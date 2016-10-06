using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StrawmanDBLibray.Classes;
using StrawmanDBLibray.Entities;

namespace StrawmanApp.Helpers
{
    public class StrawmanDBLibrayData
    {
        public static object Get(string table, bool cache)
        {
            object ret = null;
            int _year = PeriodUtil.Year;
            int _month = PeriodUtil.Month;
            switch (table)
            {
                case StrawmanDataTables.BOY_CONFIG:
                    if (!cache)
                    {
                        List<BOY_CONFIG> lst = (List<BOY_CONFIG>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<BOY_CONFIG> lst = (List<BOY_CONFIG>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<BOY_CONFIG>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.MARKET_MASTER:
                    if (!cache)
                    {
                        List<MARKET_MASTER> lst = (List<MARKET_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<MARKET_MASTER> lst = (List<MARKET_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<MARKET_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.BRAND_MASTER:
                    if (!cache)
                    {
                        List<BRAND_MASTER> lst = (List<BRAND_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<BRAND_MASTER> lst = (List<BRAND_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<BRAND_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.ROSETTA_LOADER:
                    if (!cache)
                    {
                        List<ROSETTA_LOADER> lst = (List<ROSETTA_LOADER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<ROSETTA_LOADER> lst = (List<ROSETTA_LOADER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<ROSETTA_LOADER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.GROUP_CONFIG:
                    if (!cache)
                    {
                        List<GROUP_CONFIG> lst = (List<GROUP_CONFIG>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<GROUP_CONFIG> lst = (List<GROUP_CONFIG>) Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<GROUP_CONFIG>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.GROUP_TYPES:
                    if (!cache)
                    {
                        List<GROUP_TYPES> lst = (List<GROUP_TYPES>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<GROUP_TYPES> lst = (List<GROUP_TYPES>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<GROUP_TYPES>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.GROUP_MASTER:
                    if (!cache)
                    {
                        List<GROUP_MASTER> lst = (List<GROUP_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<GROUP_MASTER> lst = (List<GROUP_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<GROUP_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_MONTH:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_MONTH_DATA> lst = (List<v_WRK_MARKET_MONTH_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table,_year,_month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_MONTH_DATA> lst = (List<v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_YTD:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_YTD_DATA> lst = (List<v_WRK_MARKET_YTD_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_YTD_DATA> lst = (List<v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, Helpers.Session.GetSession(table));
                            lst = (List<v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_MAT:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_MAT_DATA> lst = (List<v_WRK_MARKET_MAT_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_MAT_DATA> lst = (List<v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_BOY:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_BOY_DATA> lst = (List<v_WRK_MARKET_BOY_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_BOY_DATA> lst = (List<v_WRK_MARKET_BOY_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_BOY_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_BTG:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_BTG_DATA> lst = (List<v_WRK_MARKET_BTG_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_BTG_DATA> lst = (List<v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_TOTAL:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_TOTAL_DATA> lst = (List<v_WRK_MARKET_TOTAL_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_TOTAL_DATA> lst = (List<v_WRK_MARKET_TOTAL_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_TOTAL_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_MARKET_PCVSPY:
                    if (!cache)
                    {
                        List<v_WRK_MARKET_PCVSPY_DATA> lst = (List<v_WRK_MARKET_PCVSPY_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MARKET_PCVSPY_DATA> lst = (List<v_WRK_MARKET_PCVSPY_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_MARKET_PCVSPY_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_MONTH:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_MONTH_DATA> lst = (List<v_WRK_BRAND_MONTH_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_MONTH_DATA> lst = (List<v_WRK_BRAND_MONTH_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_MONTH_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_YTD:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_YTD_DATA> lst = (List<v_WRK_BRAND_YTD_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_YTD_DATA> lst = (List<v_WRK_BRAND_YTD_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_YTD_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_MAT:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_MAT_DATA> lst = (List<v_WRK_BRAND_MAT_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_MAT_DATA> lst = (List<v_WRK_BRAND_MAT_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_MAT_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_BOY:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_BOY_DATA> lst = (List<v_WRK_BRAND_BOY_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_BOY_DATA> lst = (List<v_WRK_BRAND_BOY_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_BOY_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_BTG:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_BTG_DATA> lst = (List<v_WRK_BRAND_BTG_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_BTG_DATA> lst = (List<v_WRK_BRAND_BTG_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_BTG_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_TOTAL:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_TOTAL_DATA> lst = (List<v_WRK_BRAND_TOTAL_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_TOTAL_DATA> lst = (List<v_WRK_BRAND_TOTAL_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_TOTAL_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_BRAND_PCVSPY:
                    if (!cache)
                    {
                        List<v_WRK_BRAND_PCVSPY_DATA> lst = (List<v_WRK_BRAND_PCVSPY_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_BRAND_PCVSPY_DATA> lst = (List<v_WRK_BRAND_PCVSPY_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_WRK_BRAND_PCVSPY_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                    
                case StrawmanDataTables.WRK_BOY_DATA:
                    if (!cache)
                    {
                        List<WRK_BOY_DATA> lst = (List<WRK_BOY_DATA>)StrawmanDBLibray.DBLibrary.GetBoyData();
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<WRK_BOY_DATA> lst = (List<WRK_BOY_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBoyData());
                            lst = (List<WRK_BOY_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_STRWM_MARKET_DATA:
                    if (!cache)
                    {
                        List<v_STRWM_MARKET_DATA> lst = (List<v_STRWM_MARKET_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_STRWM_MARKET_DATA> lst = (List<v_STRWM_MARKET_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetMarketData(table, _year, _month));
                            lst = (List<v_STRWM_MARKET_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_STRWM_BRAND_DATA:
                    if (!cache)
                    {
                        List<v_STRWM_MARKET_DATA> lst = (List<v_STRWM_MARKET_DATA>)StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_STRWM_MARKET_DATA> lst = (List<v_STRWM_MARKET_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetBrandData(table, _year, _month));
                            lst = (List<v_STRWM_MARKET_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_SHAREBOARD_MAT:
                    if (!cache)
                    {
                        List<v_SHAREBOARD_MAT> lst = (List<v_SHAREBOARD_MAT>)StrawmanDBLibray.DBLibrary.GetShareBoardData(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_SHAREBOARD_MAT> lst = (List<v_SHAREBOARD_MAT>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetShareBoardData(table));
                            lst = (List<v_SHAREBOARD_MAT>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_SHAREBOARD_MONTH:
                    if (!cache)
                    {
                        List<v_SHAREBOARD_MONTH> lst = (List<v_SHAREBOARD_MONTH>)StrawmanDBLibray.DBLibrary.GetShareBoardData(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_SHAREBOARD_MONTH> lst = (List<v_SHAREBOARD_MONTH>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetShareBoardData(table));
                            lst = (List<v_SHAREBOARD_MONTH>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_SHAREBOARD_YTD:
                    if (!cache)
                    {
                        List<v_SHAREBOARD_YTD> lst = (List<v_SHAREBOARD_YTD>)StrawmanDBLibray.DBLibrary.GetShareBoardData(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_SHAREBOARD_YTD> lst = (List<v_SHAREBOARD_YTD>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetShareBoardData(table));
                            lst = (List<v_SHAREBOARD_YTD>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                //Datos Franchise
                case StrawmanDataTables.v_WRK_FRANCHISE_DATA:
                    if (!cache)
                    {
                        List<v_WRK_FRANCHISE_DATA> lst = (List<v_WRK_FRANCHISE_DATA>)StrawmanDBLibray.DBLibrary.GetFrachiseData(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_FRANCHISE_DATA> lst = (List<v_WRK_FRANCHISE_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetFrachiseData(table));
                            lst = (List<v_WRK_FRANCHISE_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_WRK_FRANCHISE_MASTER:
                    if (!cache)
                    {
                        List<v_WRK_FRANCHISE_MASTER> lst = (List<v_WRK_FRANCHISE_MASTER>)StrawmanDBLibray.DBLibrary.GetFrachiseData(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_FRANCHISE_MASTER> lst = (List<v_WRK_FRANCHISE_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetFrachiseData(table));
                            lst = (List<v_WRK_FRANCHISE_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.v_WRK_MANAGEMENT_LETTERS_DATA:
                    if (!cache)
                    {
                        List<v_WRK_MANAGEMENT_LETTERS_DATA> lst = (List<v_WRK_MANAGEMENT_LETTERS_DATA>)StrawmanDBLibray.DBLibrary.GetComments(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<v_WRK_MANAGEMENT_LETTERS_DATA> lst = (List<v_WRK_MANAGEMENT_LETTERS_DATA>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetComments(table));
                            lst = (List<v_WRK_MANAGEMENT_LETTERS_DATA>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.WRK_VIEWS_VARIABLES:
                    if (!cache)
                    {
                        List<WRK_VIEWS_VARIABLES> lst = (List<WRK_VIEWS_VARIABLES>)StrawmanDBLibray.DBLibrary.GetStrawmanVariables(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<WRK_VIEWS_VARIABLES> lst = (List<WRK_VIEWS_VARIABLES>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanVariables(table));
                            lst = (List<WRK_VIEWS_VARIABLES>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.CHANNEL_MASTER:
                    if (!cache)
                    {
                        List<CHANNEL_MASTER> lst = (List<CHANNEL_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<CHANNEL_MASTER> lst = (List<CHANNEL_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<CHANNEL_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.MARKET_GROUPS:
                    if (!cache)
                    {
                        List<MARKET_GROUPS> lst = (List<MARKET_GROUPS>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<MARKET_GROUPS> lst = (List<MARKET_GROUPS>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<MARKET_GROUPS>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.USER_ROLE_TYPES:
                    if (!cache)
                    {
                        List<USER_ROLE_TYPES> lst = (List<USER_ROLE_TYPES>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<USER_ROLE_TYPES> lst = (List<USER_ROLE_TYPES>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<USER_ROLE_TYPES>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.USERS_ROLES:
                    if (!cache)
                    {
                        List<USERS_ROLES> lst = (List<USERS_ROLES>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<USERS_ROLES> lst = (List<USERS_ROLES>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<USERS_ROLES>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.USERS_PERMISSIONS:
                    if (!cache)
                    {
                        List<USERS_PERMISSIONS> lst = (List<USERS_PERMISSIONS>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<USERS_PERMISSIONS> lst = (List<USERS_PERMISSIONS>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<USERS_PERMISSIONS>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.MENU_MASTER:
                    if (!cache)
                    {
                        List<MENU_MASTER> lst = (List<MENU_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<MENU_MASTER> lst = (List<MENU_MASTER>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<MENU_MASTER>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                case StrawmanDataTables.MENU_CONFIG:
                    if (!cache)
                    {
                        List<MENU_CONFIG> lst = (List<MENU_CONFIG>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table);
                        ret = lst.ToList();
                    }
                    else
                    {
                        List<MENU_CONFIG> lst = (List<MENU_CONFIG>)Helpers.Session.GetSession(table);
                        if (lst == null)
                        {
                            Session.SetSession(table, StrawmanDBLibray.DBLibrary.GetStrawmanConfig(table));
                            lst = (List<MENU_CONFIG>)Helpers.Session.GetSession(table);
                        }
                        ret = lst.ToList();
                    }
                    break;
                    
            }
            return ret;
        }
    }
}