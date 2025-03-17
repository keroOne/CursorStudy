import { Page } from '@playwright/test'

export async function setupTest(page: Page) {
  await page.goto('/')
  await page.waitForLoadState('networkidle')
}

export async function cleanupTest(page: Page) {
  // 必要に応じてクリーンアップ処理を実装
}

export async function initializeTestData(page: Page) {
  // OS種類の初期化
  await page.goto('/os-types')
  await page.waitForLoadState('networkidle')
  
  // 既存のOS種類を削除
  const deleteButtons = page.getByRole('button', { name: '削除' }).all()
  for (const button of await deleteButtons) {
    await button.click()
    await page.getByRole('button', { name: '削除' }).click()
    await page.waitForLoadState('networkidle')
  }

  // 新しいOS種類を追加
  await page.getByRole('button', { name: '新規OS種類追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規OS種類追加' }).click()
  await page.getByLabel('OS種類名').fill('Windows 10')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')
  
  await page.getByRole('button', { name: '新規OS種類追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規OS種類追加' }).click()
  await page.getByLabel('OS種類名').fill('Windows 11')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')

  // 拠点の初期化
  await page.goto('/locations')
  await page.waitForLoadState('networkidle')
  
  // 既存の拠点を削除
  const locationDeleteButtons = page.getByRole('button', { name: '削除' }).all()
  for (const button of await locationDeleteButtons) {
    await button.click()
    await page.getByRole('button', { name: '削除' }).click()
    await page.waitForLoadState('networkidle')
  }

  // 新しい拠点を追加
  await page.getByRole('button', { name: '新規拠点追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規拠点追加' }).click()
  await page.getByLabel('拠点コード').fill('LOC001')
  await page.getByLabel('拠点名').fill('本社')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')
  
  await page.getByRole('button', { name: '新規拠点追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規拠点追加' }).click()
  await page.getByLabel('拠点コード').fill('LOC002')
  await page.getByLabel('拠点名').fill('支社')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')

  // ユーザーの初期化
  await page.goto('/users')
  await page.waitForLoadState('networkidle')
  
  // 既存のユーザーを削除
  const userDeleteButtons = page.getByRole('button', { name: '削除' }).all()
  for (const button of await userDeleteButtons) {
    await button.click()
    await page.getByRole('button', { name: '削除' }).click()
    await page.waitForLoadState('networkidle')
  }

  // 新しいユーザーを追加
  await page.getByRole('button', { name: '新規ユーザー追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規ユーザー追加' }).click()
  await page.getByLabel('ADアカウント').fill('yamada.taro')
  await page.getByLabel('表示名').fill('山田太郎')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')
  
  await page.getByRole('button', { name: '新規ユーザー追加' }).waitFor({ state: 'visible' })
  await page.getByRole('button', { name: '新規ユーザー追加' }).click()
  await page.getByLabel('ADアカウント').fill('yamada.hanako')
  await page.getByLabel('表示名').fill('山田花子')
  await page.getByRole('button', { name: '保存' }).click()
  await page.waitForLoadState('networkidle')
} 