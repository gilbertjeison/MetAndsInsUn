<template>
  <div class="position-relative">
    <template v-if="isDataLoading">
      <slot name="loading">
        Loading...
      </slot>
    </template>
    <template v-else>
      <template v-if="isDataComputing">
        <slot name="computing">
          <div class="position-absolute w-100 h-100 text-center" style="z-index: 1;">
            <div class="position-sticky d-inline-block mt-5 p-3" style="top: 0;">
              Loading table values...
            </div>
          </div>
        </slot>
      </template>

      <div v-if="data.length === 0" class="alert alert-warning" role="alert">
        {{ noDataWarningText }}
      </div>

      <div v-else-if="cols.length && rows.length" class="table-responsive">
        <table class="table table-bordered" :aria-busy="isDataLoading || isDataComputing">

          <!-- Table header -->
          <thead>
            <tr
              v-for="(colField, colFieldIndex) in internalColFields"
              :key="`head-${JSON.stringify(colField)}`"
              v-if="colField.showHeader === undefined || colField.showHeader"
              >
              <!-- Top left dead zone -->
              <th
                v-if="colFieldIndex === firstColFieldHeaderIndex && rowHeaderSize > 0"
                :colspan="rowHeaderSize"
                :rowspan="colHeaderSize"
                ></th>
              <!-- Column headers -->
              <th
                v-for="(col, colIndex) in sortedCols"
                :key="JSON.stringify(col)"
                :colspan="spanSize('col', sortedCols, colIndex, colFieldIndex)"
                v-if="spanSize('col', sortedCols, colIndex, colFieldIndex) !== 0"
                >
                <slot v-if="colField.headerSlotName" :name="colField.headerSlotName" v-bind:value="col[`col-${colFieldIndex}`]">
                  Missing slot <code>{{ colField.headerSlotName }}</code>
                </slot>
                <template v-else>
                  {{ col[`col-${colFieldIndex}`] }}
                </template>
              </th>
              <!-- Top right dead zone -->
              <th
                v-if="colFieldIndex === firstColFieldHeaderIndex && rowFooterSize > 0"
                :colspan="rowFooterSize"
                :rowspan="colFooterSize"
                ></th>
            </tr>
          </thead>

          <!-- Table body -->
          <tbody>
            <tr v-for="(row, rowIndex) in sortedRows" :key="JSON.stringify(row)">
              <!-- Row headers -->
              <th
                v-for="(rowField, rowFieldIndex) in internalRowFields"
                :key="`head-${JSON.stringify(rowField)}`"
                :rowspan="spanSize('row', sortedRows, rowIndex, rowFieldIndex)"
                v-if="(rowField.showHeader === undefined || rowField.showHeader) && spanSize('row', sortedRows, rowIndex, rowFieldIndex) !== 0"
                >
                <slot v-if="rowField.headerSlotName" :name="rowField.headerSlotName" v-bind:value="row[`row-${rowFieldIndex}`]">
                  Missing slot <code>{{ rowField.headerSlotName }}</code>
                </slot>
                <template v-else>
                  {{ row[`row-${rowFieldIndex}`] }}
                </template>
              </th>
              <!-- Values -->
              <td
                v-for="col in sortedCols"
                :key="JSON.stringify(col)"
                class="text-right"
                >
                <slot v-if="$scopedSlots.value" name="value" :value="value(row, col)" :row="Object.values(row)" :col="Object.values(col)" />
                <template v-else>{{ value(row, col) }}</template>
              </td>
              <!-- Row footers (if slots are provided) -->
              <th
                v-for="(rowField, rowFieldIndex) in internalRowFieldsReverse"
                :key="`foot-${JSON.stringify(rowField)}`"
                :rowspan="spanSize('row', rows, rowIndex, internalRowFields.length - rowFieldIndex - 1)"
                v-if="rowField.showFooter && spanSize('row', rows, rowIndex, internalRowFields.length - rowFieldIndex - 1) !== 0"
                >
                <slot v-if="rowField.footerSlotName" :name="rowField.footerSlotName" v-bind:value="row[`row-${internalRowFields.length - rowFieldIndex - 1}`]">
                  Missing slot <code>{{ rowField.footerSlotName }}</code>
                </slot>
                <template v-else>
                  {{ row[`row-${internalRowFields.length - rowFieldIndex - 1}`] }}
                </template>
              </th>
            </tr>
          </tbody>

          <!-- Table footer -->
          <tfoot>
            <tr
              v-for="(colField, colFieldIndex) in internalColFieldsReverse"
              :key="`foot-${JSON.stringify(colField)}`"
              v-if="colField.showFooter">
              <!-- Bottom left dead zone -->
              <th
                v-if="colFieldIndex === firstColFieldFooterIndex && rowHeaderSize > 0"
                :colspan="rowHeaderSize"
                :rowspan="colHeaderSize"></th>
              <!-- Column footers -->
              <th
                v-for="(col, colIndex) in sortedCols"
                :key="JSON.stringify(col)"
                :colspan="spanSize('col', sortedCols, colIndex, internalColFields.length - colFieldIndex - 1)"
                v-if="spanSize('col', sortedCols, colIndex, internalColFields.length - colFieldIndex - 1) !== 0">
                <slot v-if="colField.footerSlotName" :name="colField.footerSlotName" v-bind:value="col[`col-${internalColFields.length - colFieldIndex - 1}`]">
                  Missing slot <code>{{ colField.footerSlotName }}</code>
                </slot>
                <template v-else>
                  {{ col[`col-${internalColFields.length - colFieldIndex - 1}`] }}
                </template>
              </th>
              <!-- Bottom right dead zone -->
              <th
                v-if="colFieldIndex === firstColFieldFooterIndex && rowFooterSize > 0"
                :colspan="rowFooterSize"
                :rowspan="colFooterSize"
                ></th>
            </tr>
          </tfoot>
        </table>
      </div>
    </template>
  </div>
</template>

<script>
import HashTable from './HashTable'
import { firstBy } from 'thenby'
import naturalSort from 'javascript-natural-sort'
export default {
  props: {
    data: {
      type: Array,
      default: []
    },
    rowFields: {
      type: Array,
      default: []
    },
    colFields: {
      type: Array,
      default: []
    },
    reducer: {
      type: Function,
      default: (sum, item) => sum + 1
    },
    noDataWarningText: {
      type: String,
      default: 'No hay datos para mostrar.'
    },
    isDataLoading: {
      type: Boolean,
      default: false
    }
  },
  data: function() {
    return {
      valuesHashTable: null,
      rows: [],
      cols: [],
      // Note: we don't use the rowFields/colFields props directly to trigger table render when `computeData` has finished
      internalRowFields: this.rowFields,
      internalColFields: this.colFields,
      isDataComputing: false,
      computeDataInterval: null
    }
  },
  computed: {
    // Sort rows/cols using a composed function built with thenBy.js
    sortedRows: function() {
      let composedSortFunction
      this.internalRowFields.forEach((rowField, rowFieldIndex) => {
        if (rowFieldIndex === 0) {
          composedSortFunction = firstBy('row-0', { cmp: rowField.sort || naturalSort })
        } else {
          composedSortFunction = composedSortFunction.thenBy(`row-${rowFieldIndex}`, { cmp: rowField.sort || naturalSort })
        }
      })
      return [...this.rows].sort(composedSortFunction)
    },
    sortedCols: function() {
      let composedSortFunction
      this.internalColFields.forEach((colField, colFieldIndex) => {
        if (colFieldIndex === 0) {
          composedSortFunction = firstBy('col-0', { cmp: colField.sort || naturalSort })
        } else {
          composedSortFunction = composedSortFunction.thenBy(`col-${colFieldIndex}`, { cmp: colField.sort || naturalSort })
        }
      })
      return [...this.cols].sort(composedSortFunction)
    },
    // Compound property for watch single callback
    fields: function() {
      return [this.colFields, this.rowFields]
    },
    // Reversed props for footer iterators
    internalRowFieldsReverse: function() {
      return this.internalRowFields.slice().reverse()
    },
    internalColFieldsReverse: function() {
      return this.internalColFields.slice().reverse()
    },
    // Number of row header columns
    rowHeaderSize: function() {
      return this.internalRowFields.filter(rowField => rowField.showHeader === undefined || rowField.showHeader).length
    },
    // Number of row footer columns
    rowFooterSize: function() {
      return this.internalRowFields.filter(rowField => rowField.showFooter).length
    },
    // Number of col header rows
    colHeaderSize: function() {
      return this.internalColFields.filter(colField => colField.showHeader === undefined || colField.showHeader).length
    },
    // Number of col footer rows
    colFooterSize: function() {
      return this.internalColFields.filter(colField => colField.showFooter).length
    },
    // Index of the first column field header to show - used for table header dead zones
    firstColFieldHeaderIndex: function() {
      return this.internalColFields.findIndex(colField => colField.showHeader === undefined || colField.showHeader)
    },
    // Index of the first column field footer to show - used for table footer dead zones
    firstColFieldFooterIndex: function() {
      return this.internalColFieldsReverse.findIndex(colField => colField.showFooter)
    }
  },
  methods: {
    // Get value from valuesHashTable
    value: function(row, col) {
      return this.valuesHashTable.get({...row, ...col}) || 0
    },
    // Get colspan/rowspan size
    spanSize: function(type, values, valueIndex, fieldIndex) {
      // If left value === current value
      // and top value === 0 (= still in the same top bracket)
      // The left td will take care of the display
      if (valueIndex > 0 &&
        values[valueIndex - 1][`${type}-${fieldIndex}`] === values[valueIndex][`${type}-${fieldIndex}`] &&
        (fieldIndex === 0 || (this.spanSize(type, values, valueIndex, fieldIndex - 1) === 0))) {
        return 0
      }
      // Otherwise, count entries on the right with the same value
      // But stop if the top value !== 0 (= the top bracket has changed)
      let size = 1
      let i = valueIndex
      while (i + 1 < values.length &&
        values[i + 1][`${type}-${fieldIndex}`] === values[i][`${type}-${fieldIndex}`] &&
        (fieldIndex === 0 || (i + 1 < values.length && this.spanSize(type, values, i + 1, fieldIndex - 1) === 0))) {
        i++
        size++
      }
      return size
    },
    // Called when fields have changed => recompute cols/rows/values
    computeData: function() {
      this.isDataComputing = true
      // Start a task to avoid blocking the page
      clearInterval(this.computeDataInterval)
      this.computeDataInterval = setTimeout(() => {
        const rows = []
        const cols = []
        const valuesHashTable = new HashTable()
        this.data.forEach(item => {
          // Update rows/cols
          const rowKey = {}
          this.rowFields.forEach((field, index) => {
            rowKey[`row-${index}`] = field.getter(item)
          })
          if (!rows.some(row => {
            return this.rowFields.every((rowField, index) => row[`row-${index}`] === rowKey[`row-${index}`])
          })) {
            rows.push(rowKey)
          }
          const colKey = {}
          this.colFields.forEach((field, index) => {
            colKey[`col-${index}`] = field.getter(item)
          })
          if (!cols.some(col => {
            return this.colFields.every((colField, index) => col[`col-${index}`] === colKey[`col-${index}`])
          })) {
            cols.push(colKey)
          }
          // Update valuesHashTable
          const key = { ...rowKey, ...colKey }
          const previousValue = valuesHashTable.get(key) || 0
          valuesHashTable.set(key, this.reducer(previousValue, item))
        })
        this.internalRowFields = this.rowFields
        this.internalColFields = this.colFields
        this.rows = rows
        this.cols = cols
        this.valuesHashTable = valuesHashTable
        this.isDataComputing = false
      }, 0)
    }
  },
  watch: {
    fields: function() {
      this.computeData()
    },
    data: function() {
      this.computeData()
    }
  },
  created: function() {
    this.computeData()
  }
}
</script>

<style lang="scss" scoped>

   @import '~bootstrap/scss/bootstrap.scss'; 
td {
  min-width: 40px;
}

th {
  min-width: 80px;
}

$enable-rounded: false;
  $table-cell-padding: .5rem; // default in bs5
  $table-cell-padding-sm: .25rem; // default in bs5
  // @import '~bootstrap/scss/bootstrap.scss';

  /* Table with aria-busy attribute */
  table[aria-busy='true'] {
    opacity: 0.6;
  }

  /* FontAwesome icons */
  .svg-inline--fa.fa-fw {
    width: 1.25em;
  }

  .svg-inline--fa {
    display: inline-block;
    font-size: inherit;
    height: 1em;
    overflow: visible;
    vertical-align: -.125em !important;
  }

  .fa-pulse {
    animation: fa-spin 1s infinite steps(8);
  }

  @keyframes fa-spin {
    0% {
      transform: rotate(0deg);
    }

    to {
      transform: rotate(1turn);
    }
  }

  table {
      font-size: 11px;
      border-collapse: collapse;
      width: 100%;
  }

  td, th {
      border: 1px solid #dddddd;
      text-align: left;
      padding: 2px;
  }
  th
  {
    background-color:#2196f3 ;
    color:white;
  }
  tbody th:nth-child(2)
  {
    position:sticky;
    left:0px;
    z-index: 1;
  }

   tbody th:nth-child(3)
  {
    position:sticky;
    left:60px;
    z-index: 1;
  }
  .table thead th {
    text-align: center;
  }
  
  .v-chip .v-chip__content{
    z-index: 0;
  }

  .table th{
    vertical-align: middle;
  }
</style>