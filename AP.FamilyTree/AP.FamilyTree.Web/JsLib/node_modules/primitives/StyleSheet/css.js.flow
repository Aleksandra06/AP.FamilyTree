// @flow

const POINTER_EVENTS_AUTO_CLASSNAME = '__style_pea';
const POINTER_EVENTS_BOX_NONE_CLASSNAME = '__style_pebn';
const POINTER_EVENTS_BOX_ONLY_CLASSNAME = '__style_pebo';
const POINTER_EVENTS_NONE_CLASSNAME = '__style_pen';

// reset unwanted styles
const CSS_RESET_RULES = [
  'html{font-family:sans-serif;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;-webkit-tap-highlight-color:rgba(0,0,0,0)}',
  'body{margin:0}',
  'button::-moz-focus-inner,input::-moz-focus-inner{border:0;padding:0}',
  `input::-webkit-inner-spin-button,input::-webkit-outer-spin-button,
  input::-webkit-search-cancel-button,input::-webkit-search-decoration,
  input::-webkit-search-results-button,input::-webkit-search-results-decoration{display:none}`,
];

const CSS_HELPER_RULES = [
  `.${POINTER_EVENTS_AUTO_CLASSNAME}, .${POINTER_EVENTS_BOX_ONLY_CLASSNAME}, .${POINTER_EVENTS_BOX_NONE_CLASSNAME} * {pointer-events:auto}\n`,
  `.${POINTER_EVENTS_NONE_CLASSNAME}, .${POINTER_EVENTS_BOX_ONLY_CLASSNAME} *, .${POINTER_EVENTS_NONE_CLASSNAME} {pointer-events:none}`,
];

const styleAsClassName = {
  pointerEvents: {
    auto: POINTER_EVENTS_AUTO_CLASSNAME,
    'box-none': POINTER_EVENTS_BOX_NONE_CLASSNAME,
    'box-only': POINTER_EVENTS_BOX_ONLY_CLASSNAME,
    none: POINTER_EVENTS_NONE_CLASSNAME,
  },
};

export const getDefaultStyleSheetRules = () => CSS_RESET_RULES.concat(CSS_HELPER_RULES);

export const getStyleAsHelperClassName = (prop: string, value: string): ?string =>
  styleAsClassName[prop] && styleAsClassName[prop][value];
