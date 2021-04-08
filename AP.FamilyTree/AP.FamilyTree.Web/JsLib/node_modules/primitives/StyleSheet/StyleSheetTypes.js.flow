// @flow

type Atom = number | bool | Object | Array<?Atom>;
export type StyleObj = Atom;

export type Styles = { [key: string]: Object };
export type StyleSheet<S: Styles> = { [key: $Keys<S>]: number };

export type StyleSheetFunctions = {
  create: <S: Styles>(obj: S) => StyleSheet<S>,
  flatten: (style: ?StyleObj) => ?Object,
  resolve: (props: Object) => ?Object,
};
