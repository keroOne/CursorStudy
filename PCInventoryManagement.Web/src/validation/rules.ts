export const required = (value: any) => !!value || '必須項目です。'

export const maxLength = (max: number) => (value: string) =>
  !value || value.length <= max || `${max}文字以内で入力してください。`

export const minLength = (min: number) => (value: string) =>
  !value || value.length >= min || `${min}文字以上で入力してください。`

export const managementNumber = (value: string) =>
  !value || /^[A-Z0-9-]+$/.test(value) || '管理番号は英数字とハイフンのみ使用できます。'

export const modelName = (value: string) =>
  !value || /^[A-Za-z0-9\s-]+$/.test(value) || 'モデル名は英数字、スペース、ハイフンのみ使用できます。'

export const displayName = (value: string) =>
  !value || /^[A-Za-z0-9\s\-ぁ-んァ-ヶー一-龠]+$/.test(value) || '表示名は英数字、スペース、ハイフン、漢字、ひらがな、カタカナのみ使用できます。' 