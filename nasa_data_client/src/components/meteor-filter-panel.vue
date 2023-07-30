<template>
  <div class="d-flex flex-wrap pa-6">
    <div class="font-weight-black d-flex align-self-center">Метеориты</div>
    <v-spacer></v-spacer>
    <v-combobox
      v-model="filter.fromYears"
      :items="filterOption.years"
      label="Год с"
      @change="filterApply"
      :clearable="true"
      outlined
      dense
    ></v-combobox>
    <v-spacer></v-spacer>
    <v-combobox
      v-model="filter.toYears"
      :items="[...filterOption.years.toReversed()]"
      label="Год по"
      @change="filterApply"
      :clearable="true"
      outlined
      dense
    ></v-combobox>
    <v-spacer></v-spacer>
    <v-combobox
      :class="{
        'v-text-field__details': false,
      }"
      v-model="filter.classSelect"
      :items="filterOption.classes"
      label="Класс метеорита"
      @change="filterApply"
      :clearable="true"
      outlined
      dense
    ></v-combobox>
    <v-spacer></v-spacer>
    <v-text-field
      v-model="filter.searchName"
      @keydown.enter="filterApply"
      append-outer-icon="mdi-magnify"
      @click:append-outer="filterApply"
      label="Поиск по названию"
      single-line
      hide-details
    ></v-text-field>
  </div>
</template>

<style>
.v-text-field__details {
  display: none !important;
}
</style>

<script>
import { APICtrl } from '../services/APICtrl'

export default {
  name: 'MeteorFilterPanel',
  emits: ['filterApply'],
  mounted() {
    APICtrl.getFilter().then((data) => this.filterOptionsLoaded(data))
  },
  data() {
    return {
      filterOption: {
        years: [],
        classes: [],
      },
      filter: {
        fromYears: '',
        toYears: '',
        classSelect: '',
        searchName: '',
      },
    }
  },
  methods: {
    filterApply() {
      this.$nextTick(() => {
        this.normolizeInputYear()
        this.$emit('filterApply', this.filter)
      })
    },
    filterOptionsLoaded(filter) {
      if (filter) {
        this.filterOption = filter
        if (this.filterOption.years.length > 0) {
          this.filter.fromYears = this.filterOption.years[0]
          this.filter.toYears = this.filterOption.years[this.filterOption.years.length - 1]
        }
      }
    },
    normolizeInputYear() {
      this.filter.toYears = this.filter.toYears
        ?.replace(/[^+\d]/g, '')
        .substring(0, 4)
        .padStart(4, '0')

      this.filter.fromYears = this.filter.fromYears
        ?.replace(/[^+\d]/g, '')
        .substring(0, 4)
        .padStart(4, '0')
    },
  },
}
</script>
