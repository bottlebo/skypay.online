import axios from 'axios'
const host = ''//'/skypay'
const _companies = [
    { Id: 1, Name: "Магазин обуви" },
    { Id: 2, Name: "Магазин игрушек" }
]
const _stocks = [
    { id: 1, Name: "Склад обуви №1", companyId: 1 },
    { id: 2, Name: "Склад обуви №2", companyId: 1 },
    { id: 3, Name: "Склад игрушек №1", companyId: 2 },
    { id: 4, Name: "Склад игрушек №2", companyId: 2 }
]
const _categories = [
    { id: 1, Name: "Телевизоры", hasChild: true, companyId: 1 },
    { id: 2, Name: "Телевизоры LED", hasChild: false, parentID: 1, companyId: 1 },
    { id: 3, Name: "Телевизоры PLAZMA", hasChild: false, parentID: 1, companyId: 1 },
    //
    { id: 4, Name: "Кофеварки", hasChild: true, companyId: 2 },
    { id: 5, Name: "Esspresso", hasChild: false, parentID: 4, companyId: 2 },
]
const _cat = [
    { id: 1, title: "Телевизоры", hasChild: true, companyId: 1 },
    { id: 2, title: "Телевизоры LED", hasChild: false, parentID: 1, companyId: 1 },
    { id: 3, title: "Телевизоры PLAZMA", hasChild: true, parentID: 1, companyId: 1 },
    { id: 30, title: "Телевизоры PLAZMA-1", hasChild: false, parentID: 3, companyId: 1 },
    { id: 31, title: "Телевизоры PLAZMA-2", hasChild: false, parentID: 3, companyId: 1 },
    //
    { id: 4, title: "Кофеварки", hasChild: true, companyId: 2 },
    { id: 5, title: "Esspresso", hasChild: false, parentID: 4, companyId: 2 },
]
const _products = [
    { id: 1, Name: "Panasonic 321", Barcode: "232323", Number: '1', categoryId: 2 },
    { id: 2, Name: "Panasonic 421", Barcode: "232324", Number: '2', categoryId: 2 },
    { id: 3, Name: "Panasonic 521", Barcode: "232325", Number: '3', categoryId: 2 },
    { id: 4, Name: "Panasonic 621", Barcode: "232326", Number: '4', categoryId: 2 },
    { id: 5, Name: "Panasonic x-321", Barcode: "332323", Number: '11', categoryId: 30 },
    { id: 6, Name: "Panasonic x-421", Barcode: "332324", Number: '12', categoryId: 30 },
    { id: 7, Name: "Panasonic x-521", Barcode: "332325", Number: '13', categoryId: 31 },
    { id: 8, Name: "Panasonic x-621", Barcode: "332326", Number: '14', categoryId: 31 },
]
const _properties = [
    { id: 1, Name: "Цвет", categoryId: 2 }
]
const _documents = [
    { id: 1, Name: "Цвет", stockId: 1, Type: 1 },
    { id: 2, Name: "Цвет", stockId: 2, Type: 1 }

]
const _documentItems = [
    { id: 1, Name: "Цвет", documentId: 4 },
    { id: 2, Name: "Цвет", documentId: 4 }

]
const _units = [
    { id: 1, ShortName: "Шт.", FullName: "Штуки" },
    { id: 2, ShortName: "Кг.", FullName: "Килограмм" }
]

const _get = function (url) {
    return new Promise(function (resolve, reject) {
        axios.get(url, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {

            resolve(response.data);
        })
            .catch(function (error) {
                reject(error);
            });
    });
}
const _delete = function (url) {
    return new Promise(function (resolve, reject) {
        axios.delete(url, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {
            resolve(response.data);
        })
            .catch(function (error) {
                reject(error);
            });
    });
}
const _update = function (url, data) {
    return new Promise(function (resolve, reject) {
        axios.put(url, data, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {
            resolve(response.data);
        })
            .catch(function (error) {
                reject(error);
            });
    });
}

const reports = {
    shortstock: function () {
        return _get(host + '/reports/ShortStock')
    },
    shortsale: function () {
        return _get(host + '/reports/ShortSale')
    },
    byCategories: function () {
        return _get(host + '/reports/StockByCategories')

    },
    saleByCategories: function () {
        return _get(host + '/reports/SaleByCategories')

    },
    saleByProducts: function () {
        return _get(host + '/reports/SaleByProducts')

    },
    byValue: function () {
        return _get(host + '/reports/StockByValues')

    },
    saleByDays: function () {
        return _get(host + '/reports/SaleByDays')

    }
}
const stocks = {
    get: function (companyId) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/Stocks?$filter=CompanyId eq ${companyId}`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    }
}
const documents = {
    get: function (stockId, type) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/Documents/` + type + `?$filter=StockId eq ${stockId}&$expand=Agent`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    updateItem: function (item) {
        return _update(host + `/odata/DocumentItem(${item.id})`, item)
    },
    deleteItem: function (id) {
        return _delete(host + `/odata/DocumentItem(${id})`)
    },
    getItems: function (id) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/Document(${id})/DocumentItems?$expand=Product($expand=Unit)`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    addItem: function (item) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/DocumentItem', item, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    add: function (document) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/Document', document, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    update: function (document) {
        return new Promise(function (resolve, reject) {
            axios.put(host + '/odata/Documents(' + document.id + ')', document, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    lock: function (id) {
        return new Promise(function (resolve, reject) {
            axios.put(host + `/odata/Document(${id})/Lock`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    delete: function (id) {
        return _delete(host + `/odata/Document(${id})`)
    }
}
const agents = {
    get: function (companyId, type) {
        return new Promise(function (resolve, reject) {
            let typefilter = ''
            if (type != 0) {
                typefilter = `/${type}`
            }
            axios.get(host + `/odata/Agents` + typefilter + `?$filter=CompanyId eq ${companyId}`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    add: function (agent) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/Agents', agent, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    update: function (agent) {
        return new Promise(function (resolve, reject) {
            axios.put(host + '/odata/Agents(' + agent.id + ')', agent, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    }
}

const productset = {
    get: function (categoryId) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/ProductSets?$filter=categoryId eq ${categoryId}`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    add: function (productset) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/ProductSet', productset, {
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    update: function (productset) {
        return new Promise(function (resolve, reject) {
            axios.put(host + '/odata/ProductSet(' + productset.id + ')', productset, {
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    delete: function (id) {
        return new Promise(function (resolve, reject) {
            axios.delete(host + `/odata/ProductSet(${id})`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    getItems: function (id) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/ProductSet(${id})/ProductSetItems?$expand=Product`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    addItem: function (id, item) {
        return new Promise(function (resolve, reject) {
            axios.post(host + `/odata/ProductSet(${id})/AddProduct`, item, {
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    deleteItem: function (id) {
        return new Promise(function (resolve, reject) {
            axios.delete(host + `/odata/ProductSetItem(${id})`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    }
}
const units = {
    get: function () {
        return _units.map((el) => { return { label: el.ShortName, value: el.id } })
    }
}
const products = {
    add: function (product) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/Product', product, {
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    update: function (product) {
        return new Promise(function (resolve, reject) {
            axios.put(host + '/odata/Products(' + product.Product.id + ')', product, {
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    delete: function (id) {
        return new Promise(function (resolve, reject) {
            axios.delete(host + '/odata/Product(' + id + ')', {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    getProductCategoryProps: function (id) {
        return new Promise(function (resolve, reject) {
            axios.get(host + `/odata/Products(${id})/ProductCategoryProps`, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {

                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    getCompanyProducts: function (companyId) {
        return new Promise(function (resolve, reject) {
            axios.get(host +
                `/odata/Products?$expand=Category&$filter=Category/CompanyId eq ${companyId}`, {
                    headers: { 'Content-Type': 'application/json' }
                }).then(function (response) {

                    resolve(response.data);
                })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    getStockProducts: function (stockId) {
        return _get(host + `/odata/ProductInStock?$filter=StockId eq ${stockId}&$expand=Product`)
    }
}
const categories = {
    createCategory: function (category) {
        return new Promise(function (resolve, reject) {
            axios.post(host + '/odata/Categories', category, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    updateCategory: function (id, category) {
        return new Promise(function (resolve, reject) {
            axios.put(host + '/odata/Categories(' + id + ')', category, {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    },
    deleteCategory: function (id) {
        return new Promise(function (resolve, reject) {
            axios.delete(host + '/odata/Category(' + id + ')', {
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                resolve(response.data);
            })
                .catch(function (error) {
                    reject(error);
                });
        });
    }
}
const addCategoryProperty = function (prop) {
    return new Promise(function (resolve, reject) {
        axios.post(host + '/odata/CategoryProps', prop, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {

            resolve(response.data);
        })
            .catch(function (error) {
                reject(error);
            });
    });
}
const getCategoryProperties = function (categoryId) {
    return new Promise(function (resolve, reject) {
        axios.get(host + '/odata/Categories(' + categoryId + ')/Properties', {
        }).then(function (response) {
            resolve(response.data);
        })
            .catch(function (error) {
                reject(error);
            });
    });
}
const getCategories = function (companyId) {
    return axios.get(host + '/odata/Companies(' + companyId + ')/Categories')
}
const getProducts = function (categoryId) {
    let promiseObj = new Promise(function (resolve, reject) {
        window.setTimeout(function () {
            resolve(_products.filter(p => p.categoryId == categoryId))
        }, 4000)
    });
    return promiseObj;
}
const getCompanies = function () {
    return axios.get(host + '/odata/Companies/')
}
const getStocks = function (companyId) {
    let promiseObj = new Promise(function (resolve, reject) {
        window.setTimeout(function () {
            resolve(_stocks.filter(s => s.companyId == companyId))
        }, 100)
    });
    return promiseObj;
}
export default {
    host,
    reports,
    stocks,
    agents,
    productset,
    categories,
    units,
    products,
    getCompanies,
    documents,
    getProducts,
    getCategoryProperties,
    addCategoryProperty
}
