<template>
  <v-card>
    <meteor-filter-panel @filterApply="filterApply"></meteor-filter-panel>
    <v-data-table
      :headers="headers"
      :items="meteoriteDataList"
      :sort-by="'year'"
      :sort-desc="true"
      :options.sync="options"
      :loading="loading"
      class="elevation-1"
    ></v-data-table>
  </v-card>
</template>

<script>
import { APICtrl } from '../services/APICtrl'
import MeteorFilterPanel from './meteor-filter-panel.vue'

export default {
  name: 'MeteorTable',
  components: {
    'meteor-filter-panel': MeteorFilterPanel,
  },
  data() {
    return {
      loading: false,
      options: {},
      meteoriteDataList: [],
      filter: {},
      headers: [
        { text: 'Год', value: 'year', align: 'start' },
        { text: 'Количество метеоритов', value: 'count' },
        { text: 'Суммарная масса', value: 'sumMass' },
      ],
    }
  },
  watch: {
    options: {
      handler() {
        this.sortApply()
      },
      deep: true,
    },
  },
  methods: {
    filterApply(filter) {
      this.loading = true
      this.filter = filter
      APICtrl.getFiltered(filter).then((data) => this.tableDataLoaded(data))
    },
    tableDataLoaded(data) {
      if (data) {
        this.meteoriteDataList = data
      }
      this.loading = false
    },
    sortApply() {
      this.loading = true

      this.filter.sortBy = this.options.sortBy.length ? this.options.sortBy[0] : ''
      this.filter.sortDesc = this.options.sortDesc.length ? this.options.sortDesc[0] : ''

      APICtrl.getFiltered(this.filter).then((data) => this.tableDataLoaded(data))
    },
  },
}
</script>
