export class APICtrl {
  static async getFiltered({ fromYears = '', toYears = '', classSelect = '', searchName = '', sortBy = '', sortDesc = '',}) 
  {
    let url = new URL(`${process.env.VUE_APP_HOST}/Meteorite/GetFilteredList`)

    let params = {
      FromYears: fromYears ?? '',
      ToYears: toYears ?? '',
      ClassSelect: classSelect ?? '',
      SearchName: searchName ?? '',
      SortBy: sortBy ?? '',
      SortDesc: sortDesc ?? '',
    }
    Object.keys(params).forEach((key) => url.searchParams.append(key, params[key]))
    return await this.sendGetRequest(url)
  }

  static async getFilter() {
    let url = new URL(`${process.env.VUE_APP_HOST}/Meteorite/GetFilter`)
    return await this.sendGetRequest(url)
  }

  static async sendGetRequest(url) {
    try {
      let response = await fetch(url)

      if (response.ok) {
        return response.json()
      }
      alert(response.statusText)
    } catch (ex) {
      alert('Request failed. Check sended request')
    }
    return null
  }
}
